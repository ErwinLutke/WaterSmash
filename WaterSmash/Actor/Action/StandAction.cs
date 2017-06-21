using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class StandAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;

        private KeyboardState oldState;

        public StandAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Up)) 
            {
                // Change to JumpAction
                _actionStateMachine.Change("jump");
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                // Change to CrouchAction
                _actionStateMachine.Change("crouch");
            }
            else if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right)) // If left or right key is pressed
            {
                // change to MoveAction
                _actionStateMachine.Change("move"); 
            }
            else if (oldState.IsKeyUp(Keys.Space) && state.IsKeyDown(Keys.Space))
            {
                // Change to ThrowAction if actor is currently not throwing
                if (!_actor.isThrowing)
                {
                    _actionStateMachine.Change("throw", "stand");
                } 
            }

            oldState = state;
        }

        public void Leaving()
        {

        }
    }
}