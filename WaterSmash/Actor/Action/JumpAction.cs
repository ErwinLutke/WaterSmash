using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class JumpAction : IAction
    {
        private AActor actor; // Holds current actor
            
        private bool hasJumped = false; // Holds wether actor has jumped

        private Vector2 position; // Holds actor's position
        private Vector2 velocity; // Holds air movement velocity

        public JumpAction(AActor actor)
        {
            this.actor = actor;
        }

        public void Entered(params object[] args)
        {
            
        }

        public void HandleInput()
        {

        }

        public void Leaving()
        {

        }

        public void Update(GameTime gameTime)
        {
            position = actor.position; // Set current position
            position += velocity; 

            KeyboardState state = Keyboard.GetState();

            if(state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Space)) 
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

            if (hasJumped == false) // player is not currently jumping
            {
                position.Y -= 10f; // substract from Y -> go up
                velocity.Y = -5f; // velocity kicks in to not be a rocket
                actor.isJumping = true; // set isJumping in actor to true -> used for controlling the switch to other movements
                hasJumped = true; // set hasJumped to true to avoid double jumping
            }

            if(state.IsKeyUp(Keys.Space) || hasJumped == true)
            {
                float i = 1;
                velocity.Y += 0.15f * i;
            }

            // CHANGE 960 ACCORDING TO HITBOX SHIT
            if(position.Y >= 960) // if back on the ground
            {         
                hasJumped = false; // set hasJumped on false so that player is able to jump again
                velocity.Y = 0f; // Reset velocity
                actor.isJumping = false; // set isJumping in actor to false -> used for controlling the switch to other movements
            }

            actor.position = position;
        }
    }
}