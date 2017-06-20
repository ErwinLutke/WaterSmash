using System;
namespace Water
{
    public class Generator
    {
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

        public Generator() { }
        

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

    }
}