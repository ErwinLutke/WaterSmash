using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class MoveAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;
        private KeyLocker _keyLocker;

        private Vector2 position; // Holds current actor position

        public MoveAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
            _keyLocker = new KeyLocker();
        }
        
        public void Entered(params object[] args)
        {
            position = _actor.Position; // Set or update current actor position
            _actor.currentSpriteAnimation = "move";
        }

        KeyboardState oldState;
        public void HandleInput(KeyboardState state)
        {
            if(state.IsKeyDown(Keys.Space))
            {
                _actionStateMachine.Change("jump");
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("crouch");
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                MoveRight();
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                MoveLeft();
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("crouch");
            }
            else
            {
                _actionStateMachine.Change("stand");
            }
        }

        public void MoveRight()
        {
            position.X += 2f; // Increment X position (move right)
        }

        public void MoveLeft()
        {
            position.X -= 2f; // Decrement X position (Move left)
        }

        public void Update(GameTime gameTime)
        {
            _actor.Position = position; // Update position in actor
        }

        public void Leaving()
        {
            _actor.spriteAnimations["move"].Reset();
        }

    }
}