using System;
using Microsoft.Xna.Framework;

namespace Water
{
    internal class CrouchAction : IAction
    {
        private AActor aActor;
        //test
        public CrouchAction(AActor aActor)
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