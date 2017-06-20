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
    abstract class AActor
    {
        [DataMember]
        string name { get; set; }
        Texture2D spriteSheet;
        

        [DataMember]
        protected Inventory inventory;

        public ActionStateMachine actionStateMachine;

        bool _isInvunerable = false;

        public int health { get; set; }
        public int attack { get; set; }
        public int defense { get; set;}

        public Vector2 position { get; set; } // Holds current position of actor

        public Dictionary<string, SpriteAnimation> spriteAnimations;
        public string currentSpriteAnimation;
        
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        private GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();

        public AActor()
        {
            inventory = new Inventory();
            actionStateMachine = new ActionStateMachine();
            spriteAnimations = new Dictionary<string, SpriteAnimation>();
            
            actionStateMachine.Add("move", new MoveAction(this));
            actionStateMachine.Add("stand", new StandAction(this));
            actionStateMachine.Add("jump", new JumpAction(this));
            actionStateMachine.Add("attack", new AttackAction(this));
            actionStateMachine.Add("throw", new ThrowAction(this));
            actionStateMachine.Add("crouch", new CrouchAction(this));
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
    
        public void HandleInput(KeyboardState state)
        {
            actionStateMachine.HandleInput(state);
        }

        public void Update(GameTime gameTime)
        {
            actionStateMachine.Update(gameTime);
            spriteAnimations[currentSpriteAnimation].Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Rectangle sprite = new Rectangle(26, 24, 55, 106);
            //Rectangle playerPos = new Rectangle(position.ToPoint().X, position.ToPoint().Y - sprite.Height, sprite.Width, sprite.Height);

            //Viewport viewport = graphics.Viewport;

            // TEMP - debugging
            spriteBatch.DrawString(spriteFont, actionStateMachine.Current.ToString(), new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(spriteFont, "X " + position.X.ToString(), new Vector2(100, 200), Color.White);
            spriteBatch.DrawString(spriteFont, "Y " + position.Y.ToString(), new Vector2(200, 200), Color.White);

            //Rectangle rect = new Rectangle(position.ToPoint().X, position.ToPoint().Y - texture.Height, texture.Width, texture.Height);
            //spriteBatch.Draw(texture, rect, Color.White);

            spriteAnimations[currentSpriteAnimation].Draw(spriteBatch, position);

        }


    }
}
