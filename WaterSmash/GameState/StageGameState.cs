using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        AActor player = new Player();

        SpriteBatch spriteBatch;

        private Vector2 startPosition; // Holds player starting position 

        public StageGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
            spriteBatch = new SpriteBatch(graphics);
            
            _stages = new Dictionary<string, Stage>();
            _stages.Add("1", new Stage());
            _stages.Add("2", new Stage());

        }

        public void Draw(GameTime gameTime)
        {
            player.Draw(gameTime);
        }

        // Set which stage should be played
        public void Entered(params object[] args)
        {
            _currentStage = _stages[args[0].ToString()];

            startPosition = new Vector2(0, graphics.Viewport.Width / 2); // Set player starting position

            player.position = startPosition; // Parse starting position to player


        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Escape)) gameStateManager.Change("worldmap");
        }

        public void Leaving()
        {

        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }
    }
}
