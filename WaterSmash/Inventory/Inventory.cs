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

        public int capacity { get; set; }

        public Inventory(int capacity)
        {
            this.capacity = capacity;
            items = new List<AEquipable>();
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
