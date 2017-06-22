using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Water
{
    class Enemy : AActor
    {
        private bool inRange = false;
        private int sightRange = 50;
        //private HealthBar healthbar;
        
        public Enemy()
        {

        }
        public override void HandleInput(object state)
        {
            if ((String)state == "moveLeft")
            {
                Position = new Vector2(Position.X - 1, Position.Y);

            }
            else if ((String)state == "moveRight")
            {
                Position = new Vector2(Position.X + 1, Position.Y);
            }
            else if ((String)state == "jump")
            {
                actionStateMachine.Change("jump");
            }
            
        }

        public bool isInRange()
        {
            return inRange;
        }
        public void setInRange(bool inRange)
        {
            this.inRange = inRange;
        }
        public int getSightRange()
        {
            return sightRange;
        }
        
    }

    internal class HealthBar
    {
        
    }
}
