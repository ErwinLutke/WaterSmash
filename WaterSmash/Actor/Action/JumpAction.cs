using System;
using Microsoft.Xna.Framework;

namespace Water
{
    internal class JumpAction : IAction
    {
        private AActor actor;

        public JumpAction(AActor actor)
        {
            this.actor = actor;
        }

        public void Entered(params object[] args)
        {
            throw new NotImplementedException();
        }

        public void HandleInput()
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