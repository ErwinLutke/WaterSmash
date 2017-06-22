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

        public ActionStateMachine actionStateMachine;

        bool _isInvunerable = false;

        public int health { get; set; }
        public int attack { get; set; }
        public int defense { get; set;}

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
        public enum Direction {
            LEFT,
            RIGHT
        }

        /// <summary>
        /// Holds current direction
        /// </summary>
        public Direction direction { get; set; }

        public Direction directionAtThrow { get; set; }
        public Texture2D texture { get; set; }
        public SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        private GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();
        public Dictionary<string, SpriteAnimation> spriteAnimations;
        public string currentSpriteAnimation;
        /// <summary>
        /// Health Bar
        /// </summary>
        Texture2D healthTexture;
        Rectangle healthRect;

        public AActor()
        {
            direction = Direction.RIGHT;    // Sets starting direction

            health = 100;
            inventory = new Inventory(30);
            actionStateMachine = new ActionStateMachine();
                        spriteAnimations = new Dictionary<string, SpriteAnimation>();
            

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

        public void load()
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
            spriteAnimations[currentSpriteAnimation].Update(gameTime);

            healthRect = new Rectangle((int)position.X - (texture.Width / 2), (int)position.Y - 50, health, 20);

            // If Actor currently throwing -> update throwAction whatever the current actionstate is
            if (isThrowing)
            {
                throwAction.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            Viewport viewport = graphics.Viewport;
            
            // ------------------------- TEMP -------------------------//
            spriteBatch.DrawString(spriteFont, actionStateMachine.Current.ToString(), new Vector2(100, 100), Color.Black);

            spriteBatch.DrawString(spriteFont, "X " + Position.X.ToString(), new Vector2(100, 200), Color.Black);

            spriteBatch.DrawString(spriteFont, "Y " + Position.Y.ToString(), new Vector2(200, 200), Color.Black);

            spriteBatch.DrawString(spriteFont, "Direction " + direction.ToString(), new Vector2(300, 400), Color.Black);
            // ------------------------- TEMP -------------------------//

            spriteBatch.Draw(texture, position, Color.White);

            spriteBatch.Draw(healthTexture, healthRect, Color.Red);

            // Check if actor currently is throwing
            if(!isThrowing)
            {
                // Check if current action is ThrowAction
                if (actionStateMachine.Current is ThrowAction)
                {
                    // Check if actor is able to throw again according to time delay
                    if (timeAtThrow == 0 || (gameTime.TotalGameTime.TotalSeconds - timeAtThrow) > 1.5)
                    {
                        // Set time at throw
                        timeAtThrow = gameTime.TotalGameTime.TotalSeconds;
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

            spriteBatch.End();
            spriteAnimations[currentSpriteAnimation].Draw(spriteBatch, Position);
            Size = spriteAnimations[currentSpriteAnimation].Size;
       
        }
    }
}
