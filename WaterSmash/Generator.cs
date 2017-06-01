﻿namespace Water
{
    public class Generator
    {

        private static Generator instance;

        private Generator() { }

        public static Generator Factory
        {
            get
            {
                if (instance == null)
                {
                    instance = new Generator();
                }
                return instance;
            }
        }

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


        public static Pickup generatePickup()
        {
            return null;
        }

        //public static AEquipable generateEquipable(int type)
        //{
        //    AEquipable item = null;
        //    if (type == 1)
        //    {
        //        item.name = "";
        //        item = new Cap(0, 0, 0, 0, 0);

        //    }
        //    else
        //    {
        //        item = new Label(0, 0, 0, 0, 0);
        //    }
        //    return item;
        //}

    }
}