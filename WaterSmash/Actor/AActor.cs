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


        /// <summary>
        /// Holds ThrowAction to be able to continue throwing while switching back to other actions
        /// </summary>
        public ThrowAction throwAction;

        /// <summary>
        /// Holds wether actor is currently throwing
        /// </summary>
        public bool isThrowing;
        /// <summary>
        /// Holds time in seconds from when actor starts throwing
        /// Used for delaying next throw
        /// </summary>
        public double timeAtThrow;
        /// <summary>
        /// Direction options
        /// Used for throwing direction (and sprites)
        /// </summary>
        public enum Direction
        {
            LEFT,
            RIGHT
        }
        public Direction direction { get; set; }

        public Direction directionAtThrow { get; set; }
        public Texture2D texture { get; set; }
        // <summary>
        /// Health Bar
        /// </summary>
        Texture2D healthTexture;
        Rectangle healthRect;

        public ActionStateMachine actionStateMachine;

        public Dictionary<string, SpriteAnimation> spriteAnimations;
        public string currentSpriteAnimation;


        // TEMP - debugging
        SpriteFont spriteFont;
        private ContentManager content = GameServices.GetService<ContentManager>();
    
        public AActor()
        {
            direction = Direction.RIGHT;    // Sets starting direction
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
            //healthTexture = content.Load<Texture2D>("healthbar");
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

            //healthRect = new Rectangle((int)Position.X - (texture.Width / 2), (int)Position.Y - 50, health, 20);

            // If Actor currently throwing -> update throwAction whatever the current actionstate is
            if (isThrowing)
            {
                throwAction.Update(gameTime);
            }
        }
        GameTime g = new GameTime();

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


            //spriteBatch.Draw(texture, Position, Color.White);

            //spriteBatch.Draw(healthTexture, healthRect, Color.Red);
            if (!isThrowing)
            {
                // Check if current action is ThrowAction
                if (actionStateMachine.Current is ThrowAction)
                {
                    // Check if actor is able to throw again according to time delay
                    if (timeAtThrow == 0 || (g.TotalGameTime.TotalSeconds - timeAtThrow) > 1.5)
                    {
                        // Set time at throw
                        timeAtThrow = g.TotalGameTime.TotalSeconds;
                        // Save ThrowAction to be able to continue throwing
                        throwAction = (ThrowAction)actionStateMachine.Current;

                        directionAtThrow = direction;

                        isThrowing = true;
                    }
                }
            }
            else if (isThrowing)
            {
                // Draw throwing bottle at updating position
                spriteBatch.Draw(throwAction.bottle, throwAction.bottlePosition, Color.White);

                // ------------------------- TEMP -------------------------//
                spriteBatch.DrawString(spriteFont, throwAction.bottlePosition.X.ToString(), new Vector2(200, 300), Color.White);
                spriteBatch.DrawString(spriteFont, "Y: " + throwAction.bottlePosition.Y.ToString(), new Vector2(300, 300), Color.White);
                // ------------------------- TEMP -------------------------//

                // According to direction at throw set when to stop / reset the throwaction 
                if (directionAtThrow == Direction.RIGHT)
                {
                    // CHANGE TO ON HIT OR ON GROUND
                    if (throwAction.bottlePosition.X > 1000)
                    {
                        throwAction = null;
                        isThrowing = false;
                    }
                }
                else if (directionAtThrow == Direction.LEFT)
                {
                    // CHANGE TO ON HIT OR ON GROUND
                    if (throwAction.bottlePosition.X < 0)
                    {
                        throwAction = null;
                        isThrowing = false;
                    }
                }
            }

            spriteAnimations[currentSpriteAnimation].Draw(spriteBatch, Position);
            Size = spriteAnimations[currentSpriteAnimation].Size;
        }


    }
}
