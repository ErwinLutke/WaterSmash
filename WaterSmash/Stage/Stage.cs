using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Water
{
    [DataContract]
    public class Stage
    {
        public Texture2D progressBar;
        
        Generator generator;
        public int killedEnemies = 0;//total killed enemies
        int totalEnemies = 93;//maximale aantal enemies per stage
        GameObject Floor;
        public String name { get; set; }

        [DataMember]
        public List<String> times { get; private set; }
        public Texture2D stageThumbnail { get; private set; }
        public Texture2D stageBackground { get; set; }

        private ContentManager content = GameServices.GetService<ContentManager>();
        public List<object> GameObjects = new List<object>();//holds all map blocks
        public List<object> bg = new List<object>();//holds all bg elements

        public List<object> enemies;

        public Stage()
        {
            stageBackground = content.Load<Texture2D>("Images/stages/stage_1/bg");
            progressBar = content.Load<Texture2D>("Images/stages/HealthBar2");
            Floor = new GameObject(content.Load<Texture2D>("Images/stages/floor"), new Vector2(0, 270));
            enemies = new List<object>();
            generator = new Generator();
            GameObjects = generator.generateMap();
            bg = generator.generateBackground();
            //spawnEnemies();
        }

        /// <summary>
        /// spawned alle enemies in de map. zolang er enemies gespawned kunnen worden, wordt een nieuwe enemy gemaakt en aan de list met enemies toevegoegd 
        /// </summary>
        public void spawnEnemies(Vector2 position)
        {
            Random r = new Random();
            int rInt = r.Next(0, 1000);

            int maxEnemiesInGame = 23;//maximale aantal enemies die tegelijk in het spel mogen zijn.
           
            
                //creer loop om enemies te spawnen. wanneer totale gekillde enemeies kleiner is dan het totale enemies per stage wordt een niewe enemy gespawned.
                while (killedEnemies < totalEnemies && enemies.Count() < maxEnemiesInGame)
                {
                    if (rInt < position.X + 1000)
                    {
                        //maken van nieuwe enemy
                        enemies.Add((Enemy)generator.enemyGenerator(1, new Vector2(rInt + position.X, Floor.Position.Y)));//toevoegen van de enemy aan de enemies list
                    }
                }
            
        
        }

        /// <summary>
        /// Methode om de enemies naar de speler te bewegen.
        /// checkt of de speler in een bepaalde range is van de enemy, als dit zo is wordt de enemy hier naartoe verplaatst.
        /// </summary>

        public void moveEnemies(Vector2 playerCoord)
        {
            //loop door de enemies
            foreach (Enemy enemy in enemies)
            {
                //controleer of player.position.X kleiner is dan de enemy.position.x EN de enemy in range is van de speler
                //dit betekend dat de enemy naar links moet bewegen
                if (playerCoord.X < enemy.Position.X && enemy.isInRange())
                {
                    enemy.HandleInput("moveLeft");
                    //must be replaced with enemy.actionStateMachine.Change("action_here");
                }
                //controleer of player.position.X kleiner is dan de enemy.position.x EN de enemy in range is van de speler
                //dit betekend dat de enemy naar rechts moet bewegen
                if (playerCoord.X > enemy.Position.X && enemy.isInRange())
                {
                    enemy.HandleInput("moveRight");
                    //must be replaced with enemy.actionStateMachine.Change("action_here");

                }
            }
        }

        /// <summary>
        /// checkt of de enemies in range zijn van de speler. als dit zo is call de methode om de isInRange van enemie op true te zetten.
        /// </summary>
        public void checkInRange(Vector2 playerPos)
        {
            foreach (Enemy enemy in enemies)
            {

                //if (enemy.Position.X - player.Position.X < enemy.getSightRange())//left
                // if (player.Position.X - enemy.Position.X < enemy.getSightRange())//right
                if (playerPos.X - enemy.Position.X < enemy.getSightRange() && enemy.Position.X - playerPos.X < enemy.getSightRange())
                {
                    enemy.setInRange(true);
                }
                else
                {
                    enemy.setInRange(false);
                }
            }
        }

        /// <summary>
        /// checkt de health van alle enemies, en verwijderd deze als de health kleiner is dan 0
        /// </summary>
        public void checkHealth()
        {
            //Debug.WriteLine(_currentStage.enemies.Count());
            //Debug.WriteLine(killedEnemies);
            //loop door alle enemies
            for (int i = 0; i < enemies.Count(); i++)
            {
                Enemy e = (Enemy)enemies[i];
                //check of de health van enemy[i] kleiner is dan 0
                if (e.health < 0)
                {
                    //verwijder de dode enemy uit de lijst met enemies
                    enemies.RemoveAt(i);
                    killedEnemies++;
                }
            }
        }
        public void checkProgress()
        {
            if(killedEnemies >=totalEnemies)
            {
                killedEnemies = totalEnemies;
            }
        }


    }
}
