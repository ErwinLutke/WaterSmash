using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{

    public class Camera2D : ICamera2D
    {
        private static GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private Vector2 _position;
        protected float _viewportHeight;
        protected float _viewportWidth;

        public Camera2D()
        {
            _viewportWidth = graphics.Viewport.Width;
            _viewportHeight = graphics.Viewport.Height;

            ScreenCenter = new Vector2(_viewportWidth / 2, _viewportHeight / 2);
            Scale = 1;
            MoveSpeed = 2.0f;
        }
        

        #region Properties

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public IFocusable Focus { get; set; }
        public float MoveSpeed { get; set; }

        #endregion

        public void Update(GameTime gameTime)
        {

            Point mapSize = new Point(500, 350);
            Point screenSize = graphics.Viewport.Bounds.Size;
            var scaleX = (float)screenSize.X / mapSize.X;
            var scaleY = (float)screenSize.Y / mapSize.Y;
            Matrix matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);


            // Create the Transform used by any
            // spritebatch process
            Transform = //Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        //Matrix.CreateRotationZ(Rotation) *
                   //     Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        //       Matrix.CreateScale(new Vector3(Scale, Scale, Scale)) *
                        matrix;

            Origin = ScreenCenter / Scale;

            // Move the Camera to the position that it needs to go
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _position.X += ((Focus.Position.X - Position.X) - mapSize.X / 3)* MoveSpeed * delta;
            _position.Y += ((Focus.Position.Y - Position.Y) - mapSize.X / 2)* MoveSpeed * delta;
        }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects
        /// directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns>
        ///     <c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInView(Vector2 position, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen

            if ((position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X))
                return false;

            // If the object is not within the vertical bounds of the screen
            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
                return false;

            // In View
            return true;
        }
    }
}
