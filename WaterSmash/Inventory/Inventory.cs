using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    public class Inventory
    {
        public List<AEquipable> items { get; set; } // Holds items 

        public List<Texture2D> slots { get; } // Holds inventory slots

        private ContentManager content = GameServices.GetService<ContentManager>();

        public Inventory()
        {
            items = new List<AEquipable>();
            slots = new List<Texture2D>();

            // ADD EMPTY INVENTORY SLOTS
            for (int i = 0; i < 30; i++)
            {
                slots.Add(content.Load<Texture2D>("inventory\\inventory_slot"));
            }
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
