using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class JumpAction : IAction
    {
        private AActor actor;

        public bool hasJumped = true;

        private Vector2 position;
        private Vector2 velocity;
        private Vector2 startPosition;

        KeyboardState oldState;

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
            position = actor.position;

            position += velocity;

            KeyboardState newState = Keyboard.GetState();

            if(newState.IsKeyDown(Keys.Right) && newState.IsKeyDown(Keys.Space))
            {
                velocity.X = 3f;
            }
            else if (newState.IsKeyDown(Keys.Left) && newState.IsKeyDown(Keys.Space))
            {
                velocity.X = -3f;
            }
            else
            {
                velocity.X = 0;
            }

            if(newState.IsKeyDown(Keys.Space) && hasJumped == false)
            {
                position.Y -= 10f;
                velocity.Y = -5f;
                hasJumped = true;
            }

            if(newState.IsKeyUp(Keys.Space) || hasJumped == true)
            {
                float i = 1;
                velocity.Y += 0.15f * i;
            }

            if(position.Y + actor.texture.Height >= 900)
            {
                hasJumped = false;
            }

            if(hasJumped == false)
            {
                velocity.Y = 0f;
            }

            actor.position = position;

            //oldState = newState;
        }
    }
}