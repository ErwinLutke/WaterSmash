using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class MoveAction : IAction
    {
        private AActor actor;   // Holds an actor
            
        private Vector2 position; // Holds current actor position

        public MoveAction(AActor actor)
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
            position = actor.position; // Set or update current actor position

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 2f; // Increment X position (move right)
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 2f; // Decrement X position (Move left)
            }

            actor.position = position; // Update position in actor

            actor.Draw(gameTime);
        }
    }
}