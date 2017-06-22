using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class StandAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;
        private KeyLocker _keyLocker;

        public StandAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
            _keyLocker = new KeyLocker();
        }

        public void Entered(params object[] args)
        {
            _actor.currentSpriteAnimation = "stand";
        }

        public void Update(GameTime gameTime)
        {

        }


        KeyboardState oldState;

        public void HandleInput(KeyboardState state)
        {
            if (!_keyLocker.KeyPressed && state.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space))
            {
                _actionStateMachine.Change("jump");
                _keyLocker.LockKey(Keys.Space);
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("crouch");
            }
            else if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right)) // If left or right key is pressed
            {
                _actionStateMachine.Change("move");
            }            
            else if (!_keyLocker.KeyPressed && state.IsKeyDown(Keys.Z) && !oldState.IsKeyDown(Keys.Z))
            {
                _actionStateMachine.Change("attack");
                _keyLocker.LockKey(Keys.Z);
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("crouch");
            }

            _keyLocker.CheckInputLock(state, Keys.Space);
            _keyLocker.CheckInputLock(state, Keys.Z);
            
            oldState = state;
        }

        public void Leaving()
        {
            _actor.spriteAnimations["stand"].Reset();
        }
    }
}