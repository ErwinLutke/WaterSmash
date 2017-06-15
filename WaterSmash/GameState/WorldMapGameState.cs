using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Water
{
    [DataContract]
    class WorldMapGameState : IGameState
    {
        SpriteFont stageNameFont;
        GraphicsDevice graphicsDevice = GameServices.GetService<GraphicsDevice>();
        ContentManager contentManager = GameServices.GetService<ContentManager>();
        SpriteBatch spriteBatch;

        /// <summary>
        /// Holds how many stages are playable
        /// </summary>
        [DataMember]
        int stageProgress { get; set; }
       
        /// <summary>
        /// Holds all stagenames
        /// </summary>
        List<string> _stageNames = new List<string>();

        /// <summary>
        /// Reference to the player
        /// </summary>
        [DataMember]
        Player player;

        /// <summary>
        /// Reference to the gameStateManager
        /// </summary>
        private GameStateManager gameStateManager;

        /// <summary>
        /// Holds all image files for easy reference. Will be loaded by the contentManager
        /// </summary>
        Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Holds all locations of the diffent stages
        /// </summary>
        List<stageLocation> stageLocations = new List<stageLocation>();

        /// <summary>
        /// Holds the current location of the viewport
        /// </summary>
        //private Point currentLoc = new Point(20, 575);
        private Point currentLoc = new Point(0, 0);

        private Point viewPortSize = new Point(320, 320);

        /// <summary>
        /// Holds the stage that is highlighted, 0 if no stage is highlighted
        /// </summary>
        private int selectedStage = 0;

        /// <summary>
        /// Creates a worldmap state thath handles
        /// </summary>
        /// <param name="gameStateManager"></param>
        public WorldMapGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
            this.spriteBatch = new SpriteBatch(graphicsDevice);

            // TEMP - set some stage names
            _stageNames.Add("stage 1 - yoshi");
            _stageNames.Add("stage 2 - Bowser thingie");

            // TEMP - set location for stage 1
            // 
            stageLocations.Add(new stageLocation(new Point(50 - (viewPortSize.X / 2), 600 - (viewPortSize.Y / 2)), new Point(300 - (viewPortSize.X / 2), 850 - (viewPortSize.Y / 2))));
            stageLocations.Add(new stageLocation(new Point(350 - (viewPortSize.X / 2), 520 - (viewPortSize.Y / 2)), new Point(560 - (viewPortSize.X / 2), 720 - (viewPortSize.Y / 2))));
        }

        public void Draw(GameTime gameTime) 
        {
            int width = graphicsDevice.Viewport.Bounds.Width;
            int height = graphicsDevice.Viewport.Bounds.Height;

            spriteBatch.Begin();
            Rectangle sourceRect = new Rectangle(currentLoc.X, currentLoc.Y, viewPortSize.X, viewPortSize.Y);
            spriteBatch.Draw(images["bg"], graphicsDevice.Viewport.Bounds, sourceRect, Color.White);

            // If we are on a selectable stage, draw the highlight of the stage
            if(selectedStage != 0)
            {
                Rectangle messageRect = new Rectangle(new Point(width / 2 - 200, height / 2 - 100), new Point(400, 200));
                spriteBatch.Draw(images["stage_selection_" + selectedStage.ToString()], graphicsDevice.Viewport.Bounds, sourceRect, Color.White);
                spriteBatch.Draw(images["message"], messageRect, Color.White);
                float nameWidth = stageNameFont.MeasureString(_stageNames[selectedStage - 1]).X;
                spriteBatch.DrawString(stageNameFont, _stageNames[selectedStage - 1], new Vector2((width / 2) - (nameWidth / 2), height / 2), Color.White);                      
            }
            spriteBatch.End();
        }


        public void Entered(params object[] args)
        {
            loadContent();

            if (player == null)
            {
                if (args.Length > 0)
                {
                    player = (Player)args[0];
                }
            }
        }

        public void HandleInput(KeyboardState state)
        {
            // Go the menu
            if (state.IsKeyDown(Keys.Escape))
            {
                gameStateManager.Change("menu");
            }

            // Moves the viewport
            if (state.IsKeyDown(Keys.Up)) currentLoc.Y -= 5;
            if (state.IsKeyDown(Keys.Down)) currentLoc.Y += 5;
            if (state.IsKeyDown(Keys.Left)) currentLoc.X -= 5;
            if (state.IsKeyDown(Keys.Right)) currentLoc.X += 5;

            // Play the stage if it is selectable
            if(state.IsKeyDown(Keys.Enter))
            {
                if(selectedStage != 0)
                {
                    gameStateManager.Change("stage", selectedStage);
                }
            }
        }

        public void Leaving()
        {
            // Clear arrays and unload content
            images.Clear();
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            selectedStage = getStageInView();

        }


        /// <summary>
        /// Checks if the viewport is on a selectable stage
        /// </summary>
        /// <returns>Returns stage number, else returns 0</returns>
        private int getStageInView()
        {
            foreach (stageLocation area in stageLocations)
            {
                if (currentLoc.X > area.start.X 
                    && currentLoc.X < area.end.X 
                    && currentLoc.Y > area.start.Y 
                    && currentLoc.Y < area.end.Y)
                {
                    return stageLocations.IndexOf(area) + 1;
                }

            }
            return 0;
        }

        /// <summary>
        /// Loads all neccesary content. Graphics and audio
        /// </summary>
        private void loadContent()
        {
            images.Add("bg", contentManager.Load<Texture2D>("Images/worldmap"));
            images.Add("message", contentManager.Load<Texture2D>("Images/message"));
            images.Add("stage_selection_1", contentManager.Load<Texture2D>("Images/stage_selection_1"));
            images.Add("stage_selection_2", contentManager.Load<Texture2D>("Images/stage_selection_2"));

            stageNameFont = contentManager.Load<SpriteFont>("Font/StageName");
        }
    }


    /// <summary>
    /// Holds data for where a stage is on the worldmap
    /// </summary>
    class stageLocation
    {
        /// <summary>
        /// This holds the starting coordinates (Top left)
        /// </summary>
        public Point start;

        /// <summary>
        /// This holds the ending coordinates (Bottom right)
        /// </summary>
        public Point end;

        public stageLocation(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
