using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Water
{
    internal class JumpAction : IAction
    {
        private AActor _actor; // Holds current actor
        private ActionStateMachine _actionStateMachine;
            
        private bool hasJumped = false; // Holds wether actor has jumped

        private Vector2 ground;
        private Vector2 position; // Holds actor's position
        private Vector2 velocity; // Holds air movement velocity

        public JumpAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = _actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {
            position = _actor.Position; // Set current position
            ground = position;
            _actor.currentSpriteAnimation = "jump";
        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Space))
            {
                velocity.X = 3f; // add velocity, go right
            }
            else if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Space))
            {
                velocity.X = -3f; // substract velocity, go left
            }
            else // if no space key pressed
            {
                if (state.IsKeyDown(Keys.Left)) // continue going left if left key is pressed
                {
                    velocity.X = -3f;
                }
                else if (state.IsKeyDown(Keys.Right)) // continue going to right if right key is pressed
                {
                    velocity.X = 3f;
                }
                else // if no space or left or right key is pressed, stay at same position
                {
                    velocity.X = 0;
                }
            }

            if (state.IsKeyUp(Keys.Space) || hasJumped == true)
            {
                float i = 1;
                velocity.Y += 0.15f * i;
            }
        }

        public void Update(GameTime gameTime)
        {
            position += velocity; 
         

            if (hasJumped == false) // player is not currently jumping
            {
                position.Y -= 10f; // substract from Y -> go up
                velocity.Y = -5f; // velocity kicks in to not be a rocket
                hasJumped = true; // set hasJumped to true to avoid double jumping
            }
        
            // CHANGE 960 ACCORDING TO HITBOX SHIT
            if(position.Y > ground.Y) // if back on the ground
            {
                position.Y = ground.Y;
                hasJumped = false; // set hasJumped on false so that player is able to jump again
                velocity.Y = 0f; // Reset velocity
                _actionStateMachine.Change("stand");
           }

            _actor.Position = position;
        }


        public void Leaving()
        {
            _actor.spriteAnimations["jump"].Reset();
        }

    }
}