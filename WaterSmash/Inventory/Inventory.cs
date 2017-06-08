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
        
        public Inventory()
        {
            items = new List<AInventoryObject>();
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
