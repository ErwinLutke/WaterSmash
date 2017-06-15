using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Water
{
    abstract class AInventoryObject
    {
        public String name { get; set; }

        public Texture2D texture { get; set; }

        protected ContentManager content = GameServices.GetService<ContentManager>();

        public Boolean isSelected { get; set; }

        public Vector2 position { get; set; }
    }
}