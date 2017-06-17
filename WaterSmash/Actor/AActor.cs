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

        ActionStateMachine asm;

        [DataMember]
        protected Inventory inventory;

        public ActionStateMachine fsm;

        bool _isInvunerable = false;

        public int health { get; set; }
        public int attack { get; set; }
        public int defense { get; set;}


        public Vector2 position { get; set; }

        public Texture2D texture { get; set; }

        public IAction currentAction { get; set; }

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        private GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();

        public AActor()
        {
            inventory = new Inventory();
            fsm = new ActionStateMachine();

            spriteBatch = new SpriteBatch(graphics);

            fsm.Add("move", new MoveAction(this));
            fsm.Add("stand", new StandAction(this));
            fsm.Add("jump", new JumpAction(this));
            fsm.Add("attack", new AttackAction(this));
            fsm.Add("throw", new ThrowAction(this));
            fsm.Add("crouch", new CrouchAction(this));
            fsm.Change("stand");

            texture = content.Load<Texture2D>("inventory\\lable");
            spriteFont = content.Load<SpriteFont>("inventory\\inventory");
        }


        public Inventory GetInventory()
        {
            return inventory;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            Viewport viewport = graphics.Viewport;

            spriteBatch.DrawString(spriteFont, fsm.currentAction.ToString(), new Vector2(100, 100), Color.Black);

            spriteBatch.Draw(texture, position, Color.White);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            fsm.Update(gameTime);
        }

        public void HandleInput()
        {
            fsm.HandleInput();
        }

    }
}
