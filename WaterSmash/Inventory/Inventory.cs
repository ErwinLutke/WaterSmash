using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    class Inventory
    {
        public List<AEquipable> items { get; set; } // Holds items 

        private ContentManager content;

        public Texture2D slot;

        public int capacity { get; set; }

        public Inventory(int capacity)
        {
            this.capacity = capacity;

            items = new List<AEquipable>();
            content = GameServices.GetService<ContentManager>();
            slot = content.Load<Texture2D>("inventory\\inventory_slot");
        }

        public void AddInventoryObject(AEquipable item)
        {
            items.Add(item);
        }

        public void RemInventoryObject(AEquipable item)
        {
            items.Remove(item);
        }
    }
}
