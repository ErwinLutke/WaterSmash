using System;
using Microsoft.Xna.Framework;

namespace Water
{
    internal class ThrowAction : IAction
    {
        private AActor aActor;

        public ThrowAction(AActor aActor)
        {
            this.aActor = aActor;
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