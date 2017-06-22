using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Water
{
    class SpriteAnimation
    {
        public Texture2D Texture { get; set; }
        public int Speed { get; set; }
        public Vector2 Size { get; set; }
      
        private int sequenceStep;
        private int totalFrames;

        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 2000;

        private List<int> spriteSequence;

        public SpriteAnimation(Texture2D texture, int sprites, int animationSpeed)
        {
            Texture = texture;
            sequenceStep = 0;
            totalFrames = sprites;
            Speed = animationSpeed;
            spriteSequence = new List<int>();
        }

        public void setSpriteSequence(List<int> sequence)
        {
            spriteSequence = sequence;
        }

        public void Reset()
        {
            sequenceStep = 0;
            timeSinceLastFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame / Speed)
            {
                timeSinceLastFrame -= millisecondsPerFrame / Speed;
                sequenceStep++;
                if (sequenceStep == spriteSequence.Count) sequenceStep = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / totalFrames;
            int height = Texture.Height;
            int column = spriteSequence[sequenceStep];
            Size = new Vector2(width, height);

            Rectangle sourceRectangle = new Rectangle(width * column, 0, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y - height, width, height);
            
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
