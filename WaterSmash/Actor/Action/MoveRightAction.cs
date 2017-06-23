using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    class MoveRightAction : IAction
    {
        private AActor _actor; // Holds current actor
        private ActionStateMachine _actionStateMachine;

        private Vector2 position; // Holds actor's position

        public MoveRightAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = _actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {
            position = _actor.Position; // Set current position
            _actor.currentSpriteAnimation = "moveRight";
        }

        public void HandleInput(KeyboardState state)
        {
            {
                if (state.IsKeyDown(Keys.Right))
                {
                    moveRight();
                }
                else if (state.IsKeyDown(Keys.Left))
                {
                    _actionStateMachine.Change("moveLeft");
                }
                else
                {
                    _actionStateMachine.Change("stand");
                }
            }
        }

        private void moveRight()
        {
            _actor.direction = AActor.Direction.RIGHT; // Set facing position to right
            position.X += 2f; // Increment X position (move right)
        }

        public void Leaving()
        {
            _actor.Position = position; // Update position in actor
        }

        public void Update(GameTime gameTime)
        {
            _actor.spriteAnimations["moveRight"].Reset();
        }
    }
}
