using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class CrouchAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;

        private Vector2 position; // Holds actor's position

        public CrouchAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {
            position = _actor.Position; // Set current position
            _actor.currentSpriteAnimation = "crouch";
        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Down) && state.IsKeyDown(Keys.Right))
            {
                MoveRight();
            }
            if (state.IsKeyDown(Keys.Down) && state.IsKeyDown(Keys.Left))
            {
                MoveLeft();
            }

            if (!state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("stand");
            }

        }

        public void MoveRight()
        {

            _actor.direction = AActor.Direction.RIGHT; // Set facing position to right
            position.X += 0.7f; // Increment X position (move right)

        }

        public void MoveLeft()
        {
            if (position.X <= 255) { }
            else
            {
                _actor.direction = AActor.Direction.LEFT; // Set facing position to left
                position.X -= 0.7f; // Decrement X position (Move left)
            }
        }

        public void Update(GameTime gameTime)
        {
            _actor.Position = position;
        }

        public void Leaving()
        {
            _actor.spriteAnimations["crouch"].Reset();
        }
    }
}