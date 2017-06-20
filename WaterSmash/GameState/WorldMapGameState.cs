using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace Water
{
    [DataContract]
    class WorldMapGameState : IGameState
    {
        GraphicsDevice graphicsDevice = GameServices.GetService<GraphicsDevice>();
        ContentManager contentManager = GameServices.GetService<ContentManager>();

        SpriteFont stageNameFont;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Holds how many stages are playable
        /// NOT USED YET
        /// </summary>
        [DataMember]
        int stageProgress { get; set; }
       
        /// <summary>
        /// Holds all data of the stages
        /// </summary>
        List<StageData> stageData = new List<StageData>();

        /// <summary>
        /// Reference to the player
        /// </summary>
        [DataMember]
        Player player;

        /// <summary>
        /// The background music
        /// </summary>
        private Song worldMusic;

        /// <summary>
        /// Reference to the gameStateManager
        /// </summary>
        private GameStateManager gameStateManager;

        /// <summary>
        /// Holds all image files for easy reference. Will be loaded by the contentManager
        /// </summary>
        Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();
       
        /// <summary>
        /// Holds the current location of the viewport
        /// </summary>
        private Point currentLoc = new Point(130, 295);

        /// <summary>
        /// Sets the size of the viewport (this is how zoomed in we are in the worldmap)
        /// </summary>
        private Point viewPortSize = new Point(150, 150);

        /// <summary>
        /// Holds the stage that is highlighted, 0 if no stage is highlighted
        /// </summary>
        private int selectedStage = 0;

        /// <summary>
        /// Holds the previous keyboardstate. used to check if a key is pressed or not
        /// </summary>
        KeyboardState oldState;

        /// <summary>
        /// Holds the key that has been pressed
        /// </summary>
        private Keys pressedKey;

        /// <summary>
        /// Used to check if a key has been pressed
        /// </summary>
        private bool keyPressed = false;


        public WorldMapGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
            this.spriteBatch = new SpriteBatch(graphicsDevice);

            loadStageData();
        }

        public void Entered(params object[] args)
        {
            loadContent();
            MediaPlayer.Play(worldMusic);
            MediaPlayer.IsRepeating = true;

            if (player == null)
            {
                if (args.Length > 0)
                {
                    player = (Player)args[0];
                }
                else
                {
                    player = new Player();
                }
            }

            if(gameStateManager.Previous is MenuGameState)
            {
                // load player here
                Debug.WriteLine("came from menu, OOOEOOEOEEE");
            }
            else if(gameStateManager.Previous is StageGameState)
            {
                if(args.Length > 0)
                {
                    stageData[selectedStage - 1].record = (string)args[0];
                }
            }

           
        }

        public void HandleInput(KeyboardState state)
        {
            handleViewPortMovement(state);

            // Check if we can press the specified key again
            checkInputLock(state, Keys.Enter);
            checkInputLock(state, Keys.Escape);
            checkInputLock(state, Keys.I);

            // set the oldstate as the current state to prepare for the next state
            oldState = state;
        }

        public void Update(GameTime gameTime)
        {
            // check if the viewport contains a stage
            selectedStage = getStageInView();
        }

        public void Draw(GameTime gameTime)
        {
            int width = graphicsDevice.Viewport.Bounds.Width;
            int height = graphicsDevice.Viewport.Bounds.Height;

            // Begin drawing and disable AA for pixally art
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, null);

            // We only want to draw a certain part of the worldmap depending on the zoom level (viewPortSize)
            Rectangle sourceRect = new Rectangle(currentLoc.X - (viewPortSize.X / 2), currentLoc.Y - (viewPortSize.Y / 2), viewPortSize.X, viewPortSize.Y);
            spriteBatch.Draw(images["worldmap"], graphicsDevice.Viewport.Bounds, sourceRect, Color.White);
            spriteBatch.Draw(images["fog"], graphicsDevice.Viewport.Bounds, Color.White);

            // If we are on a selectable stage, draw the highlight of the stage
            if (selectedStage != 0)
            {
                // draw the selection highlight of a stage
                spriteBatch.Draw(stageData[selectedStage - 1].selected, graphicsDevice.Viewport.Bounds, sourceRect, Color.White);

                // get the widths and height of the text to draw on screen
                float stageLevelWidth = stageNameFont.MeasureString("-= Stage " + stageData[selectedStage - 1].level + " =-").X;
                float stageNameHeight = stageNameFont.MeasureString(stageData[selectedStage - 1].name).Y;
                float stageNameWidth = stageNameFont.MeasureString(stageData[selectedStage - 1].name).X;
                float stageRecordWidth = stageNameFont.MeasureString("Best Record: " + stageData[selectedStage - 1].record).X;

                // create black outline
                spriteBatch.DrawString(stageNameFont, "-= Stage " + stageData[selectedStage - 1].level + " =-", new Vector2((width / 2) - (stageLevelWidth / 2) - 2, (height / 2) - 2 - stageNameHeight * 6), Color.Black);
                spriteBatch.DrawString(stageNameFont, "-= Stage " + stageData[selectedStage - 1].level + " =-", new Vector2((width / 2) - (stageLevelWidth / 2) + 2, (height / 2) - 2 - stageNameHeight * 6), Color.Black);
                spriteBatch.DrawString(stageNameFont, "-= Stage " + stageData[selectedStage - 1].level + " =-", new Vector2((width / 2) - (stageLevelWidth / 2) + 2, (height / 2) + 2 - stageNameHeight * 6), Color.Black);
                spriteBatch.DrawString(stageNameFont, "-= Stage " + stageData[selectedStage - 1].level + " =-", new Vector2((width / 2) - (stageLevelWidth / 2) - 2, (height / 2) + 2 - stageNameHeight * 6), Color.Black);
                // draw text containing which state this is
                spriteBatch.DrawString(stageNameFont, "-= Stage " + stageData[selectedStage - 1].level + " =-", new Vector2((width / 2) - (stageLevelWidth / 2), (height / 2) - stageNameHeight * 6), Color.White);

                // create black outline
                spriteBatch.DrawString(stageNameFont, stageData[selectedStage - 1].name, new Vector2((width / 2) - (stageNameWidth / 2) - 2, (height / 2) - 2 - stageNameHeight * 5), Color.Black);
                spriteBatch.DrawString(stageNameFont, stageData[selectedStage - 1].name, new Vector2((width / 2) - (stageNameWidth / 2) + 2, (height / 2) - 2 - stageNameHeight * 5), Color.Black);
                spriteBatch.DrawString(stageNameFont, stageData[selectedStage - 1].name, new Vector2((width / 2) - (stageNameWidth / 2) + 2, (height / 2) + 2 - stageNameHeight * 5), Color.Black);
                spriteBatch.DrawString(stageNameFont, stageData[selectedStage - 1].name, new Vector2((width / 2) - (stageNameWidth / 2) - 2, (height / 2) + 2 - stageNameHeight * 5), Color.Black);
                // draw text containing the name of the stage
                spriteBatch.DrawString(stageNameFont, stageData[selectedStage - 1].name, new Vector2((width / 2) - (stageNameWidth / 2), (height / 2) - stageNameHeight * 5), Color.White);

                // create black outline
                spriteBatch.DrawString(stageNameFont, "Best Record: " + stageData[selectedStage - 1].record, new Vector2((width / 2) - (stageRecordWidth / 2) - 2, (height / 2) - 2 - stageNameHeight * 4), Color.Black);
                spriteBatch.DrawString(stageNameFont, "Best Record: " + stageData[selectedStage - 1].record, new Vector2((width / 2) - (stageRecordWidth / 2) + 2, (height / 2) - 2 - stageNameHeight * 4), Color.Black);
                spriteBatch.DrawString(stageNameFont, "Best Record: " + stageData[selectedStage - 1].record, new Vector2((width / 2) - (stageRecordWidth / 2) + 2, (height / 2) + 2 - stageNameHeight * 4), Color.Black);
                spriteBatch.DrawString(stageNameFont, "Best Record: " + stageData[selectedStage - 1].record, new Vector2((width / 2) - (stageRecordWidth / 2) - 2, (height / 2) + 2 - stageNameHeight * 4), Color.Black);
                // draw text containing the record of the stage
                spriteBatch.DrawString(stageNameFont, "Best Record: " + stageData[selectedStage - 1].record, new Vector2((width / 2) - (stageRecordWidth / 2), (height / 2) - stageNameHeight * 4), Color.White);

                
            }

            spriteBatch.End();
        }

        public void Leaving()
        {
            // Clear arrays and unload content
            images.Clear();
            contentManager.Unload();
        }

        /// <summary>
        /// Checks if the viewport is on a selectable stage
        /// </summary>
        /// <returns>Returns stage number, else returns 0</returns>
        private int getStageInView()
        {
            foreach (StageData stage in stageData)
            {
                if (currentLoc.X > stage.startLocation.X 
                    && currentLoc.X < stage.endLocation.X 
                    && currentLoc.Y > stage.startLocation.Y 
                    && currentLoc.Y < stage.endLocation.Y)
                {
                    return stage.level;
                }

            }
            return 0;
        }

        /// <summary>
        /// Loads all neccesary content. Graphics and audio
        /// </summary>
        private void loadContent()
        {
            images.Add("worldmap", contentManager.Load<Texture2D>("Images/worldmap"));
            images.Add("fog", contentManager.Load<Texture2D>("Images/fog"));
            images.Add("message", contentManager.Load<Texture2D>("Images/message"));

            stageNameFont = contentManager.Load<SpriteFont>("Font/StageName");

            worldMusic = contentManager.Load<Song>("audio/worldmap");

            foreach (StageData stage in stageData)
            {
                stage.selected = contentManager.Load<Texture2D>("Images/stages/stage_" + stage.level + "/worldmap_selected_" + stage.level);
                stage.preview = contentManager.Load<Texture2D>("Images/stages/stage_" + stage.level + "/worldmap_preview_" + stage.level);
            }
        }

        /// <summary>
        /// Loads all data necessary for the stages
        /// 
        /// NOTE - should be gotten from xml
        /// </summary>
        private void loadStageData()
        {
            StageData stage_1 = new StageData();
            StageData stage_2 = new StageData();

            stage_1.level = 1;
            stage_1.name = "The Docks";
            stage_1.description = "First stage";
            stage_1.record = "--'--\"---";
            stage_1.startLocation = new Point(130, 255);
            stage_1.endLocation = new Point(190, 295);

            stage_2.level = 2;
            stage_2.name = "Cabin in the woods";
            stage_2.description = "Second stage";
            stage_2.record = "--'--\"---";
            stage_2.startLocation = new Point(180, 175);
            stage_2.endLocation = new Point(240, 215);

            stageData.Add(stage_1);
            stageData.Add(stage_2);
        }

        /// <summary>
        /// Handles the movement of the viewport, what button does what
        /// </summary>
        /// <param name="state">The current state of the keyboard</param>
        private void handleViewPortMovement(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Up)) currentLoc.Y -= 1;
            else if (state.IsKeyDown(Keys.Down)) currentLoc.Y += 1;
            if (state.IsKeyDown(Keys.Left)) currentLoc.X -= 1;
            else if (state.IsKeyDown(Keys.Right)) currentLoc.X += 1;

            // Zoom in
            if (state.IsKeyDown(Keys.OemPlus))
            {
                viewPortSize.X -= 5;
                viewPortSize.Y -= 5;
            }
            // zoom out
            else if (state.IsKeyDown(Keys.OemMinus))
            {
                viewPortSize.X += 5;
                viewPortSize.Y += 5;
            }

            // Go the menu
            if (!keyPressed && state.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape))
            {
                gameStateManager.Change("menu");
                lockKey(Keys.Escape);
            }
            // Go the inventory
            else if (!keyPressed && state.IsKeyDown(Keys.I) && !oldState.IsKeyDown(Keys.I))
            {
                gameStateManager.Change("inventory", player);
                lockKey(Keys.I);
            }

            // Play the stage if it is selectable
            else if (!keyPressed && state.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
            {
                if (selectedStage != 0)
                {
                    gameStateManager.Change("stage", selectedStage, player);
                    lockKey(Keys.Enter);
                }
            }
        }

        /// <summary>
        /// Locks the specified key so that it can only be pressed once
        /// </summary>
        /// <param name="key">The key to lock</param>
        private void lockKey(Keys key)
        {
            pressedKey = key;
            keyPressed = true;
        }

        /// <summary>
        /// Checks if the key is no longer being pressed and releases its lock
        /// </summary>
        /// <param name="state">The current keyboardstate to check against the key</param>
        /// <param name="key">The key to check</param>
        private void checkInputLock(KeyboardState state, Keys key)
        {
            if (keyPressed && pressedKey == key && !state.IsKeyDown(key) && !oldState.IsKeyDown(key))
            {
                keyPressed = false;
            }
        }
    }


    /// <summary>
    /// Holds all information of a stage necessary for the worldmap
    /// </summary>
    internal class StageData
    {
        /// <summary>
        /// This holds the starting coordinates (Top left)
        /// </summary>
        public Point startLocation;

        /// <summary>
        /// This holds the ending coordinates (Bottom right)
        /// </summary>
        public Point endLocation;

        /// <summary>
        /// The name of the stage
        /// </summary>
        public string name;

        /// <summary>
        /// The description of the stage
        /// </summary>
        public string description;

        /// <summary>
        /// Which stage it is
        /// </summary>
        public int level;

        /// <summary>
        /// Holds the highlight to render when the stage is hovered on
        /// </summary>
        public Texture2D selected;

        /// <summary>
        /// Holds the preview to show when the stage is hovered on
        /// </summary>
        public Texture2D preview;

        /// <summary>
        /// Holds the record of the stage
        /// </summary>
        public string record;
    }
}
