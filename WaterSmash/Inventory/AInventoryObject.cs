using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Water
{
    public abstract class AInventoryObject
    {
        public String name { get; set; }

        public Texture2D texture { get; set; }

        protected ContentManager content = GameServices.GetService<ContentManager>();

        public Boolean isSelected { get; set; } // Holds wether item is selected or not - used for inventory

        public Vector2 position { get; set; } // Holds position of item - used for inventory
    }
}