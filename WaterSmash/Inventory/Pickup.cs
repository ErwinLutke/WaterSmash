using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    public class Pickup : AInventoryObject, IDroppable
    {
        public enum PickupType {
            HEALTH = 0,
            MANA = 1
        }
        
        public enum Amount
        {
            SMALL,
            MEDIUM,
            LARGE
        }

        public PickupType type { get; set; }
        public Amount amount { get; set; }
        //public int amount { get; set; }

        public Pickup(PickupType type)
        {
            this.type = type;
        }
    }
}
