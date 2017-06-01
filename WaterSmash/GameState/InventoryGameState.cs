using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    class InventoryGameState : IGameState
    {
        private GameStateManager gameStateManager;
        AActor player;

        public InventoryGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Entered(params object[] args)
        {
            throw new NotImplementedException();
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
