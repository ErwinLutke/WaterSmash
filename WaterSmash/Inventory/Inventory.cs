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
        public List<AInventoryObject> items { get; set; }

        public List<Texture2D> slots { get; }

        private ContentManager content = GameServices.GetService<ContentManager>();
        
        public Inventory()
        {
            items = new List<AInventoryObject>();
            slots = new List<Texture2D>();
        
            // ADD EMPTY INVENTORY SLOTS
            for(int i = 0; i < 42; i++)
            {
                slots.Add(content.Load<Texture2D>("inventory\\inventory_slot"));
            }
        }

        public void AddInventoryObject(AInventoryObject item)
        {
            items.Add(item);
        }

        public void RemInventoryObject(AInventoryObject item)
        {
            items.Remove(item);
        }
    }
}
