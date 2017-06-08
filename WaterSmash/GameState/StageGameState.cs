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
        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        // Set which stage should be played
        public void Entered(params object[] args)
        {
            _currentStage = _stages[args[0].ToString()];
        }

        public void HandleInput(KeyboardState state)
        {
            throw new NotImplementedException();
        }

        public void Leaving()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
