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

        public ThrowAction throwAction;
        public Boolean isThrowing;
        public Double elapsedTimeAfterThrow;

        public Texture2D texture { get; set; }

        public SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        private GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();

        /// <summary>
        /// Health Bar
        /// </summary>
        Texture2D healthTexture;
        Rectangle healthRect;
        public AActor()
        {
            health = 100;
            inventory = new Inventory(30);
            actionStateMachine = new ActionStateMachine();

            spriteBatch = new SpriteBatch(graphics);
         
            actionStateMachine.Add("move", new MoveAction(this));
            actionStateMachine.Add("stand", new StandAction(this));
            actionStateMachine.Add("jump", new JumpAction(this));
            actionStateMachine.Add("attack", new AttackAction(this));
            actionStateMachine.Add("throw", new ThrowAction(this));
            actionStateMachine.Add("crouch", new CrouchAction(this));
        }


        public Inventory GetInventory()
        {
            return inventory;
        }

        public void loadTextures()
        {
            texture = content.Load<Texture2D>("inventory\\lable");
            spriteFont = content.Load<SpriteFont>("inventory\\inventory");

            healthTexture = content.Load<Texture2D>("healthbar");
        }

        public void HandleInput(KeyboardState state)
        {
            actionStateMachine.HandleInput(state);
        }

        public void Update(GameTime gameTime)
        {
            actionStateMachine.Update(gameTime);

            healthRect = new Rectangle((int)position.X - (texture.Width / 2), (int)position.Y - 50, health, 20);

            if (isThrowing)
            {
                throwAction.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            Viewport viewport = graphics.Viewport;

            spriteBatch.DrawString(spriteFont, actionStateMachine.Current.ToString(), new Vector2(100, 100), Color.Black);

            spriteBatch.DrawString(spriteFont, "X " + position.X.ToString(), new Vector2(100, 200), Color.Black);

            spriteBatch.DrawString(spriteFont, "Y " + position.Y.ToString(), new Vector2(200, 200), Color.Black);

            spriteBatch.Draw(texture, position, Color.White);

            spriteBatch.Draw(healthTexture, healthRect, Color.Red);


            if(!isThrowing)
            {
                if (elapsedTimeAfterThrow == 0 || (gameTime.TotalGameTime.TotalSeconds - elapsedTimeAfterThrow) > 1.5)
                {
                    if (actionStateMachine.Current is ThrowAction)
                    {
                        //throwAction = (ThrowAction)actionStateMachine.Current;

                        throwAction = new ThrowAction(this);
                        throwAction.Entered();
                        throwAction.Throw();
                        isThrowing = true;
                    }
                }
            }
            else if (isThrowing)
            {
                if (throwAction.bottlePosition.X > 1000)
                {
                    //throwAction.bottle.Dispose();
                    throwAction = null;
                    isThrowing = false;
                    elapsedTimeAfterThrow = gameTime.TotalGameTime.TotalSeconds;
                }
                else
                {
                    spriteBatch.Draw(throwAction.bottle, throwAction.bottlePosition, Color.White);
                    spriteBatch.DrawString(spriteFont, throwAction.bottlePosition.X.ToString(), new Vector2(200, 300), Color.White);
                    spriteBatch.DrawString(spriteFont, "Y: " + throwAction.bottlePosition.Y.ToString(), new Vector2(300, 300), Color.White);
                }
            }


            spriteBatch.End();
        }
    }
}
