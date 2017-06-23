using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Water
{
    public class Generator
    {
        ContentManager content = GameServices.GetService<ContentManager>();

        /// <summary>
        /// Userd for generating randoms
        /// </summary>
        Random rnd;

        /// <summary>
        /// Preset of random item names
        /// </summary>
        string[] randomNames =
        {
            "of Sustenance Deflection",
            "of Mana and Performance",
            "of the Rite of Serenity",
            "of the Sorcery of Heat",
            "of the Killer's Conjuration of Corruption",
            "Wind Gyser",
            "of the Viper",
            "of Darkness Storm",
            "of Vicious Demons",
            "Minor Snakes",
            "Sagely Eagles",
            "Flaming",
            "Otherworldly Healers",
            "Disrupting",
            "of Water Control",
            "of the Curse of Blinding",
            "of Serpent Killing",
            "Thunderous",
            "Undead's",
            "of the Angels",
            "Mystic",
            "of Nothing and Falsehood",
            "Golden",
            "Platinum",
            "of Slime Chains",
            "of Voidness Breath",
            "of Demonic Bronze",
            "Ghostly Fiery",
            "of the Undertakers' Invocation of Flesh",
        };

        public Generator() { setSpriteAnimations(); }
        

        enum Special
        {
            FIRE = 0,
            WIND = 1,
            WATER = 2,
            EARTH = 3,
            ELECTRIC = 4,
            ICE = 5,
        }

        enum Grade
        {
            COMMON = 0,
            UNCOMMEN = 1,
            RARE = 2,
            EPIC = 3,
            LEGENDARY = 4,
            OPAF = 5
        }

        public Inventory generateInventory()
        {
            //Inventory inventory = new Inventory(4);

            rnd = new Random(Guid.NewGuid().GetHashCode());

            //for(int i = 0; i < inventory.capacity; i++)
            //{
            //    inventory.AddInventoryObject(generateEquipable(rnd.Next(2)));
            //}

            return null;
        }

        /// <summary>
        /// Generatie random pickup dropped by enemies 
        /// Can be either health or mana 
        /// Player can pick these up for replenishment
        /// </summary>
        /// <returns></returns>
        public Pickup generatePickup()
        {
            Pickup pickup = null;

            rnd = new Random(Guid.NewGuid().GetHashCode());

            int type = rnd.Next(2);
            if(type == 0)
            {
                pickup = new Pickup(Pickup.PickupType.HEALTH);
            }
            else
            {
                pickup = new Pickup(Pickup.PickupType.MANA);
            }

            int amount = rnd.Next(3);

            if(amount == 0)
            {
                pickup.amount = Pickup.Amount.SMALL;
            }
            else if(amount == 1)
            {
                pickup.amount = Pickup.Amount.MEDIUM;
            }
            else
            {
                pickup.amount = Pickup.Amount.LARGE;
            }

            return pickup;
        }

        /// <summary>
        /// Generate random equipables dropped by enemies
        /// Can be either a Cap or a Label to strenghten the players weapon
        /// Uses randomNames array to apply a random name to the item to be dropped
        /// 
        /// Also generates random stats 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AEquipable generateEquipable(int type)
        {
            AEquipable item = null;

            rnd = new Random(Guid.NewGuid().GetHashCode());

            String itemName = randomNames[rnd.Next(randomNames.Length)];

            if (type == 1)
            {
                item = new Cap(rnd.Next(10) + 1, rnd.Next(10) + 1, rnd.Next(10) + 1, rnd.Next(6), rnd.Next(6));

                if (itemName.Substring(0, 2).Equals("of"))
                {
                    item.name = "Cap " + itemName;
                }
                else
                {
                    item.name = itemName + " Cap";
                }       
            }
            else
            {
                item = new Label(rnd.Next(10) + 1, rnd.Next(10) + 1, rnd.Next(10) + 1, rnd.Next(6), rnd.Next(6));

                if (itemName.Substring(0, 2).Equals("of"))
                {
                    item.name = "Label " + itemName;
                }
                else
                {
                    item.name = itemName + " Label";
                }
            }
            return item;
        }

        /// <summary>
        /// Genereer een nieuwe enemy, afhankelijk van de dificulty
        /// </summary>
        /// <param name="dificulty">dificulty of the enemy</param>
        /// <returns>enemy with given dificulty, higher is stronger</returns>
        public object enemyGenerator(int dificulty, Vector2 pos)
        {

            int baseHealth = 100;
            int baseAttack = 12;
            int baseDefence = 12;
            int sight = 123 * (dificulty / 2);


            Enemy spawn = new Enemy();
            spawn.name = "enemieiei";
            //spawn.inventory = generateInventory();
            spawn.health = baseHealth*dificulty;
            spawn.attack = baseAttack * dificulty;
            spawn.defense = baseDefence * dificulty;
            // spawn.setSightRange(sight);
            spawn.Position = pos;

            spawn.spriteAnimations = spriteAnimations["enemy"];
            spawn.actionStateMachine.Change("stand");

            return spawn;
        }
        public object bossGenerator(int dificulty, Vector2 pos)
        {

            int baseHealth = 2;
            int baseAttack = 12;
            int baseDefence = 33;
            int sight = 123 * (dificulty / 2);


            AActor spawn = new Boss();
            spawn.name = "enemieiei";
            //spawn.inventory = generateInventory();
            spawn.health = baseHealth * dificulty;
            spawn.attack = baseAttack * dificulty;
            spawn.defense = baseDefence * dificulty;
            // spawn.setSightRange(sight);
            spawn.Position = pos;

            spawn.spriteAnimations = spriteAnimations["enemy"];
            spawn.actionStateMachine.Change("stand");

            return spawn;
        }

        /// <summary>
        /// set sprite animations for the enemy.
        /// </summary>
        Dictionary<string, Dictionary<string, SpriteAnimation>> spriteAnimations;
        private void setSpriteAnimations()
        {
            spriteAnimations = new Dictionary<string, Dictionary<string, SpriteAnimation>>();
            spriteAnimations.Add("enemy", new Dictionary<string, SpriteAnimation>());
            spriteAnimations["enemy"].Add("stand", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/stand"), 3, 10));
            spriteAnimations["enemy"].Add("attack", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/attack"), 1, 15));
            spriteAnimations["enemy"].Add("moveLeft", new SpriteAnimation(content.Load<Texture2D>("Images/characters/player/move"), 1, 20));
            spriteAnimations["enemy"]["moveLeft"].setSpriteSequence(new List<int>() { 2, 1, 0, 1, 2, 3, 4, 3 });
            spriteAnimations["enemy"]["stand"].setSpriteSequence(new List<int>() { 0, 1, 2, 1 });
            spriteAnimations["enemy"]["attack"].setSpriteSequence(new List<int>() { 0 });
        }


        /// <summary>
        /// "random" map generator, generates a straight line so far...
        /// </summary>
        /// <returns></returns>
        /// 
        private List<object> GameObjects = new List<object>();//holds all map blocks
        GameObject Floor;
        int map_x = 0;
        public List<object> generateMap()
        { 
            Floor = new GameObject(content.Load<Texture2D>("Images/stages/floor"), new Vector2(0, 270));
            int maxBlox = 100;//max amount of blocks in game
            while (GameObjects.Count < maxBlox)//loop als aantal game objects kleiner is dan maximale aantal game objects.
            {
                Vector2 testvec = new Vector2(map_x, Floor.Position.Y - 20);//
                Texture2D testobj = content.Load<Texture2D>("Images/stages/stage_1/floor");//

                GameObject test = new GameObject(testobj, testvec);//
                GameObjects.Add(test);//voeg de vloer toe aan de lijst met objects
                map_x = map_x + 100;//increment de x waarde van map_x, om het volgende object op de juist plek te spawnen
            }
            return GameObjects;
        }
        private List<object> bg = new List<object>();//holds all map blocks
        public List<object> generateBackground(int stage)
        {
            Texture2D testobj = content.Load<Texture2D>("Images/stages/stage_" +stage +"/fill");//
            int maxBlox = 30;//max amount of blocks in game
            int currentx = 0;
            while (bg.Count < maxBlox)//loop als aantal game objects kleiner is dan maximale aantal game objects.
            {
                Vector2 testvec = new Vector2(currentx, 0-70);//
                GameObject test = new GameObject(testobj, testvec);//
                bg.Add(test);//voeg de vloer toe aan de lijst met objects
                currentx = currentx + testobj.Width;
                Debug.WriteLine("wall x :" + currentx);
            }
            return bg;

        }

    }
}