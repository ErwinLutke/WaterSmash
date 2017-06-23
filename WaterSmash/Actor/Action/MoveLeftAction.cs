using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Water
{
    internal class MoveLeftAction : IAction
    {
        private AActor _actor; // Holds current actor
        private ActionStateMachine _actionStateMachine;

        private Vector2 position; // Holds actor's position

        public MoveLeftAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = _actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {
            position = _actor.Position; // Set current position
            _actor.currentSpriteAnimation = "moveLeft";
        }

        public void HandleInput(KeyboardState state)
        {
            if(state.IsKeyDown(Keys.Left))
            {
                moveLeft();
            }
            else if(state.IsKeyDown(Keys.Right))
            {
                _actionStateMachine.Change("moveRight");
            }
            else
            {
                _actionStateMachine.Change("stand");
            }
       }

        private void moveLeft()
        {
            _actor.direction = AActor.Direction.LEFT; // Set facing position to left
            position.X -= 2f; // Decrement X position (Move left)
        }

        public void Update(GameTime gameTime)
        { 
            _actor.Position = position;
        }


        public void Leaving()
        {
            _actor.spriteAnimations["moveLeft"].Reset();
        }


    }
}