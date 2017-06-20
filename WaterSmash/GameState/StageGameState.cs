using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Water
{
    class StageGameState : IGameState
    {
        private Dictionary<String, Stage> _stages;
        private Stage _currentStage;
        private Generator generator;

        private GameStateManager gameStateManager;
        private GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();

        AActor player;

        Dictionary<string, Dictionary<string, SpriteAnimation>> spriteAnimations;

        SpriteBatch spriteBatch;
        Texture2D map;
        Point mapSize;
        Matrix matrix;

        private Vector2 startPosition; // Holds player starting position 

        public StageGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
            spriteBatch = new SpriteBatch(graphics);
            spriteAnimations = new Dictionary<string, Dictionary<string, SpriteAnimation>>();
            
            _stages = new Dictionary<string, Stage>();
            _stages.Add("1", new Stage());
            _stages.Add("2", new Stage());

        }

        // Set which stage should be played
        public void Entered(params object[] args)
        {
            map = content.Load<Texture2D>("Images/stages/stage_1/map");
            mapSize = new Point(500, 350);
            Point screenSize = graphics.Viewport.Bounds.Size;
            var scaleX = (float)screenSize.X / mapSize.X;
            var scaleY = (float)screenSize.Y / mapSize.Y;
            matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            if (args.Length > 0)
            {
                _currentStage = _stages[args[0].ToString()];
                player = (Player)args[1];
            } else
            {
                player = new Player();
            }
            
            //player.position = new Vector2(screenSize.X / 20, screenSize.Y / 1.4f); // Set player starting position
            player.position = new Vector2(25, 260); // Set player starting position
            player.actionStateMachine.Change("stand");
            Debug.WriteLine(player);

            spriteAnimations.Add("player", new Dictionary<string, SpriteAnimation>());
            spriteAnimations["player"].Add("stand", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/stand"), 3, 1));
            spriteAnimations["player"].Add("move", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/move"), 5, 2));
            spriteAnimations["player"]["stand"].setSpriteSequence(new List<int>() { 0, 1, 2, 1});
            spriteAnimations["player"]["move"].setSpriteSequence(new List<int>() { 2, 3, 4, 3, 2, 1, 0, 1} );
            player.spriteAnimations = spriteAnimations["player"];

            // TEMP - debugging purpose
            player.load();
        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Escape)) gameStateManager.Change("worldmap");
            player.HandleInput(state);
        }


        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            // Begin drawing and disable AA for pixally art
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, matrix);

            spriteBatch.Draw(map, map.Bounds, Color.White);

            player.Draw(gameTime, spriteBatch);

            spriteBatch.End();

        }

        public void Leaving()
        {
            spriteAnimations.Clear();
            content.Unload();
        }
    }
}
