using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    class Pickup : AInventoryObject, IDroppable
    {
        public enum PickupType {
            HEALTH = 0,
            MANA = 1
        }

        public PickupType type { get; set; }
        public int amount { get; set; }

        public Pickup(PickupType type)
        {
            this.type = type;
        }
    }
}
