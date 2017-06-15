using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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


        ActionStateMachine fsm = new ActionStateMachine();
        bool _isInvunerable = false;

        public int health { get; set; }
        public int attack { get; set; }
        public int defense { get; set;}
        

        public AActor()
        {
            inventory = new Inventory();
            fsm = new ActionStateMachine();

            fsm.Add("move", new MoveAction(this));
            fsm.Add("stand", new StandAction(this));
            fsm.Add("jump", new JumpAction(this));
            fsm.Add("attack", new AttackAction(this));
            fsm.Add("throw", new ThrowAction(this));
            fsm.Add("crouch", new CrouchAction(this));
            fsm.Change("stand");
        }


        public Inventory GetInventory()
        {
            return inventory;
        }

        public void Draw(GameTime gameTime)
        {

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
