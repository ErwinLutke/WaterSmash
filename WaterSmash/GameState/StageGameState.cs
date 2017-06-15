using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    class StageGameState : IGameState
    {
        private Dictionary<String, Stage> _stages;
        private Stage _currentStage;
        private Generator generator;

        private GameStateManager gameStateManager;
        AActor player;

        public StageGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
            _stages = new Dictionary<string, Stage>();
            _stages.Add("1", new Stage());
            _stages.Add("2", new Stage());

        }

        public void Draw(GameTime gameTime)
        {

        }

        // Set which stage should be played
        public void Entered(params object[] args)
        {
            _currentStage = _stages[args[0].ToString()];
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

        }
    }
}
