using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class CrouchAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;

        public CrouchAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {

        }

        public void HandleInput(KeyboardState state)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Leaving()
        {

        }
    }
}