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

        KeyboardState oldState;

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

        public void HandleInput(KeyboardState state)
        {
            if(state.IsKeyDown(Keys.Up))
            {
                // Switch to JumpAction
                _actionStateMachine.Change("jump");
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                // Switch to CrouchAction
                _actionStateMachine.Change("crouch");
            }
            else if(oldState.IsKeyUp(Keys.Space) && state.IsKeyDown(Keys.Space))
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
            else if (state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("crouch");
            }
            else if (!_keyLocker.KeyPressed && state.IsKeyDown(Keys.Z) && !oldState.IsKeyDown(Keys.Z))
            {
                _actionStateMachine.Change("attack");
                _keyLocker.LockKey(Keys.Z);
            }
            else
            {
                // Switch to StandAction
                _actionStateMachine.Change("stand");
            }

            _keyLocker.CheckInputLock(state, Keys.Space);
            _keyLocker.CheckInputLock(state, Keys.Z);

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
            _actor.spriteAnimations["move"].Reset();
        }

    }
}