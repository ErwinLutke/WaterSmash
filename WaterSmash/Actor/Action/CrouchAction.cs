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
            if(!state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("stand");
            }

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Leaving()
        {
            _actor.spriteAnimations["crouch"].Reset();
        }
    }
}