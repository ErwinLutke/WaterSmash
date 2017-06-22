using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class AttackAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;

        private Vector2 position; // Holds actor's position

        private int timeSinceLastFrame;

        public AttackAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {
            position = _actor.Position; // Set current position
            _actor.currentSpriteAnimation = "attack";
        }

        public void HandleInput(KeyboardState state)
        {

        }

        public void Update(GameTime gameTime)
        {
            _actor.Position = position; // Update position in actor

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > 200)
            {
                timeSinceLastFrame -= 200;
                _actionStateMachine.Change("stand");
            }
        }

        public void Leaving()
        {
            _actor.spriteAnimations["attack"].Reset();

        }

    }
}