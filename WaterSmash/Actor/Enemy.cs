using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Water
{
     class Enemy : AActor
    {
        private bool inRange = false;
        private int sightRange = 50;
        private bool cooldown = false;
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
        public bool isOnCooldown()
        {
            return cooldown;
        }
        /// <summary>
        /// methode om de enemies niet te laten spammen met attacks
        /// </summary>
        public void coolDown()
        {
            cooldown = true;
            double coolDownTime = 5000;
            GameTime cd = new GameTime();
            while (cooldown)
            { 
                coolDownTime += cd.ElapsedGameTime.TotalMilliseconds;
                Debug.WriteLine("cooldown Time" + coolDownTime);
                if (coolDownTime >= 5000)
                {
                    cooldown = false;
                }
            }


        }
        
    }

    internal class HealthBar
    {
        
    }
}
