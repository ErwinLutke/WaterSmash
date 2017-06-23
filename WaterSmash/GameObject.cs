using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    public class GameObject
    {

        protected Texture2D texture;

        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; } // Holds current position of actor
        public Vector2 Velocity { get; set; }

        public Rectangle BoundingBox {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y - (int)Size.Y,
                    (int)Size.X,
                    (int)Size.Y);
            }
        }

        public GameObject()
        {

        }
        
        public GameObject(Texture2D texture, Vector2 Position)
        {
            this.texture = texture;
            this.Size = texture.Bounds.Size.ToVector2();
            this.Position = Position;
        }

        public GameObject(Texture2D texture, Vector2 Position, Vector2 Velocity)
        {
            this.texture = texture;
            this.Size = texture.Bounds.Size.ToVector2();
            this.Position = Position;
            this.Velocity = Velocity;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gametime)
	    {
	        spriteBatch.Draw(texture, Position, Color.White);
	    }

    }
}
