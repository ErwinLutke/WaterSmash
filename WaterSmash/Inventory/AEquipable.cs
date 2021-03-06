﻿namespace Water
{
    public abstract class AEquipable : AInventoryObject, IDroppable
    {
        public bool isEquipped = false;

        enum Special
        {
            NONE = 0,
            FIRE = 1,
            WIND = 2,
            WATER = 3,
            EARTH = 4,
            ELECTRIC = 5,
            ICE = 6,
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

        Grade grade { get; set; }
        Special special { get; set; }

        public int level { get; }
        public int attack { get; }
        public int defense { get; }

        public AEquipable (int attack, int defense, int level, int grade, int special)
        {
            this.attack = attack;
            this.defense = defense;
            this.level = level;
            this.grade = (Grade)grade;
            this.special = (Special)special;
        }


    }
}