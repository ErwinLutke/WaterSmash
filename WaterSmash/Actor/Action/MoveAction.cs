using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class MoveAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;

        private Vector2 position; // Holds current actor position

        KeyboardState oldState;

        public MoveAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {
            position = _actor.Position; // Set or update current actor position
        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Up))
            {
                // Switch to JumpAction
                _actionStateMachine.Change("jump");
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                // Switch to CrouchAction
                _actionStateMachine.Change("crouch");
            }
            else if (oldState.IsKeyUp(Keys.X) && state.IsKeyDown(Keys.X))
            {
                // if actor currently is throwing, cannot switch to trowAction
                if (!_actor.isThrowing)
                {
                    // Switch to ThrowAction
                    _actionStateMachine.Change("throw", "move");
                }
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                MoveRight();
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                MoveLeft();
            }
            else
            {
                // Switch to StandAction
                _actionStateMachine.Change("stand");
            }

            oldState = state;
        }

        public void MoveRight()
        {
            _actor.direction = AActor.Direction.RIGHT; // Set facing position to right
            position.X += 2f; // Increment X position (move right)
        }

        public void MoveLeft()
        {
            _actor.direction = AActor.Direction.LEFT; // Set facing position to left
            position.X -= 2f; // Decrement X position (Move left)
        }

        public void Update(GameTime gameTime)
        {
            _actor.Position = position; // Update position in actor
        }

        public void Leaving()
        {

        }

    }
}