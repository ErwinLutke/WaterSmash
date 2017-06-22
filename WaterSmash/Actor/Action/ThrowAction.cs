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

        /// <summary>
        /// Holds actual bottle position
        /// </summary>
        public Vector2 bottlePosition;

        /// <summary>
        /// Holds startposition of throwaction
        /// </summary>
        public Vector2 startPosition;

        SpriteBatch spriteBatch;

        /// <summary>
        /// Holds wether throwAction has reached max height and needs to fall down
        /// </summary>
        bool fallDown = false;

        /// <summary>
        /// Holds fallspeed -> increases on update
        /// </summary>
        float fallSpeed = 0.1f;

        /// <summary>
        /// Holds name of previous action
        /// </summary>
        string prevAction;

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

            Throw();

            if (args.Length > 0)
            {
                // Save previous action to switch back to after throw
                prevAction = args[0].ToString();
            }
        }

        public void HandleInput(KeyboardState state)
        {
            _actionStateMachine.Change(prevAction);

            oldState = state;
        }

        public void Throw()
        {
            bottlePosition = _actor.Position;
            bottlePosition.Y -= _actor.Size.Y;

            startPosition = bottlePosition;

            fallSpeed = 0.1f;

            if (_actor.direction == AActor.Direction.RIGHT)
            {
                velocity.X = 25f;

            }
            else if (_actor.direction == AActor.Direction.LEFT)
            {
                velocity.X = -25f;
            }

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

            if (fallDown)
            {
                velocity.Y += fallSpeed;
                fallSpeed += 2f;
            }
        }
    }
}