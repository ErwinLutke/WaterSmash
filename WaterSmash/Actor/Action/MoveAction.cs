﻿using System;
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
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 2f; // Increment X position (move right)
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 2f; // Decrement X position (Move left)
            }
            else if (state.IsKeyDown(Keys.Space))
            {
                _actionStateMachine.Change("jump");
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("crouch");
            }
            else
            {
                _actionStateMachine.Change("stand");
            }

            if (!_keyLocker.KeyPressed && state.IsKeyDown(Keys.Z) && !oldState.IsKeyDown(Keys.Z))
            {
                _actionStateMachine.Change("attack");
                _keyLocker.LockKey(Keys.Z);
            }

            _keyLocker.CheckInputLock(state, Keys.Space);
            _keyLocker.CheckInputLock(state, Keys.Z);

            oldState = state;

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