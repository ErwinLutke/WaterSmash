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

        // Set which stage should be played
        public void Entered(params object[] args)
        {
            if (args.Length > 0)
            {
                _currentStage = _stages[args[0].ToString()];
                player = (Player)args[1];
            } else
            {
                player = new Player();
            }
            
            player.position = new Vector2(50, 50); // Set player starting position
            Debug.WriteLine(player);

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
            player.Draw(gameTime);
        }

        public void Leaving()
        {

        }
    }
}
