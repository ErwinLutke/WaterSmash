﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Water
{
    internal class CrouchAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;

        private Vector2 position; // Holds current actor position

        public CrouchAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {
            position = _actor.position; // Set or update current actor position
            _actor.texture = content.Load<Texture2D>("inventory\\cap");
        }

        public void HandleInput(KeyboardState state)
        {
            if(state.IsKeyDown(Keys.Down) && state.IsKeyDown(Keys.Right))
            {
                CrouchRight();
            }
            else if (state.IsKeyDown(Keys.Down) && state.IsKeyDown(Keys.Left))
            {
                CrouchLeft();
            }
            else if(state.IsKeyDown(Keys.Down))
            {
                Crouch();
            }
            else if (state.IsKeyDown(Keys.Space))
            {
                _actionStateMachine.Change("jump");
            }
            else if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right)) // If left or right key is pressed
            {
                _actionStateMachine.Change("move");
            }
            else
            {
                _actionStateMachine.Change("stand");
            }
        }

        public void CrouchRight()
        {
            position.X += 1f; // Increment X position (move right)
        }

        public void CrouchLeft()
        {
            position.X -= 1f; // Decrement X position (Move left)
        }

        public void Crouch()
        {
            if(!state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("stand");
            }

        }

        public void Update(GameTime gameTime)
        {
            _actor.position = position; // Update position in actor
        }

        public void Leaving()
        {
            _actor.spriteAnimations["crouch"].Reset();
        }
    }
}