using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    [DataContract]
    class Player : AActor
    {
        [DataMember]
        AEquipable equippedCap;

        [DataMember]
        AEquipable equippedLabel;


        public void equipLabel(Label label)
        {
            equippedLabel = label;
        }

        public void equipCap(Cap cap)
        {
            equippedCap = cap;
        }

    }
}
