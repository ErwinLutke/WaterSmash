using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    class InventoryGameState : IGameState
    {
        private GameStateManager gameStateManager;
        private GraphicsDeviceManager graphics;
        
        //AActor player;

        public InventoryGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
        }

        public void Draw(GameTime gameTime)
        {
            
        }

        public void Entered(params object[] args)
        {
            graphics = GameServices.GetService<GraphicsDeviceManager>();
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
