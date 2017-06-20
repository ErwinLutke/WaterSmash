using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Water
{
    internal class ThrowAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;

        ContentManager content;
        GraphicsDevice graphics;

        public Vector2 velocity;

        public Texture2D bottle;

        KeyboardState oldState;

        public Vector2 bottlePosition;
        public Vector2 startPosition;

        SpriteBatch spriteBatch;

        Boolean fallDown = false;

        float fallSpeed = 0.1f;

        public ThrowAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {
            content = GameServices.GetService<ContentManager>();
            graphics = GameServices.GetService<GraphicsDevice>();

            spriteBatch = new SpriteBatch(graphics);

            bottle = content.Load<Texture2D>("Images\\Actions\\bottleThrow");

            velocity.X = 25f;

            bottlePosition = _actor.position;
            bottlePosition.Y -= _actor.texture.Height;

            startPosition = bottlePosition;
        }

        public void HandleInput(KeyboardState state)
        {
            KeyboardState newState = state;

            if (state.IsKeyDown(Keys.Up))
            {
                _actionStateMachine.Change("jump");
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("crouch");
            }
            else if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right)) // If left or right key is pressed
            {
                _actionStateMachine.Change("move");
            }

            oldState = newState;
        }

        public void Leaving()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            bottlePosition += velocity;

            if (bottlePosition.Y < (startPosition.Y - 50))
            {
                fallDown = true;
            }


            bottlePosition.Y -= 25f;
            velocity.Y = -1f;

            if(fallDown)
            {
                velocity.Y += fallSpeed;
                fallSpeed += 2f;
            }
        }
    }
}