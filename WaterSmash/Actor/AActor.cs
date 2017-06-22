using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    [DataContract]
    abstract class AActor : GameObject
    {
        [DataMember]
        string name { get; set; }

        [DataMember]
        protected Inventory inventory;

        public int health { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }

        bool _isInvunerable = false;

        //
        private static GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        Texture2D rect = new Texture2D(graphics, 100, 10);
        Color[] data;




        public ActionStateMachine actionStateMachine;

        public Dictionary<string, SpriteAnimation> spriteAnimations;
        public string currentSpriteAnimation;


        // TEMP - debugging
        SpriteFont spriteFont;
        private ContentManager content = GameServices.GetService<ContentManager>();
    
        public AActor()
        {
            health = 100;
            data = new Color[health * 10];
            inventory = new Inventory();
            actionStateMachine = new ActionStateMachine();
            spriteAnimations = new Dictionary<string, SpriteAnimation>();

            actionStateMachine.Add("move", new MoveAction(this));
            actionStateMachine.Add("stand", new StandAction(this));
            actionStateMachine.Add("jump", new JumpAction(this));
            actionStateMachine.Add("attack", new AttackAction(this));
            actionStateMachine.Add("throw", new ThrowAction(this));
            actionStateMachine.Add("crouch", new CrouchAction(this));
            actionStateMachine.Add("moveLeft",new MoveLeftAction(this));
        }

        // TEMP - debugging
        public void load()
        {
            spriteFont = content.Load<SpriteFont>("inventory\\inventory");
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        public virtual void HandleInput(object state)
        {
            actionStateMachine.HandleInput((KeyboardState)state);
        }

        int timeSinceLastFrame;
        public void Update(GameTime gameTime)
        {
            actionStateMachine.Update(gameTime);
            spriteAnimations[currentSpriteAnimation].Update(gameTime);

            if (this is Enemy)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > 2000)
                {
                    timeSinceLastFrame -= 2000;
                    actionStateMachine.Change("attack");
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle sprite = new Rectangle(26, 24, 55, 106);
            //Rectangle playerPos = new Rectangle(position.ToPoint().X, position.ToPoint().Y - sprite.Height, sprite.Width, sprite.Height);

            //Viewport viewport = graphics.Viewport;
            if (this is Player)
            {
                // TEMP - debugging
                spriteBatch.DrawString(spriteFont, actionStateMachine.Current.ToString(), new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(spriteFont, "X " + Position.X.ToString(), new Vector2(100, 200), Color.White);
                spriteBatch.DrawString(spriteFont, "Y " + Position.Y.ToString(), new Vector2(200, 200), Color.White);

            }
            else
            {
                //spriteBatch.DrawString(spriteFont, health.ToString(), new Vector2(300, 100), Color.White);
            }
            //Rectangle rect = new Rectangle(position.ToPoint().X, position.ToPoint().Y - texture.Height, texture.Width, texture.Height);
            //spriteBatch.Draw(texture, rect, Color.White);
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Green;
            rect.SetData(data);

            Vector2 coor = new Vector2(Position.X, Position.Y - 80);
            spriteBatch.Draw(rect, coor, Color.White);

            spriteAnimations[currentSpriteAnimation].Draw(spriteBatch, Position);
            Size = spriteAnimations[currentSpriteAnimation].Size;
        }


    }
}
