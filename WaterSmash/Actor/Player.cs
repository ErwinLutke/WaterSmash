using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        public Cap equippedCap; // Holds current equipped cap

        [DataMember]
        public Label equippedLabel; // Holds current equipped label

        private ContentManager content = GameServices.GetService<ContentManager>(); // TESTING PURPOSES

        public Player()
        {
            inventory.AddInventoryObject(new Label(2, 2, 2, 2, 2));
            inventory.AddInventoryObject(new Cap(1, 1, 1, 1, 1));
            inventory.AddInventoryObject(new Label(3, 3, 3, 3, 3));
            inventory.AddInventoryObject(new Cap(4, 4, 4, 4, 4));

            Cap cap = new Water.Cap(5, 5, 5, 5, 5);
            cap.texture = content.Load<Texture2D>("inventory\\cap_diff");
            inventory.AddInventoryObject(cap);
        }

        /// <summary>
        /// Swap or set equippedLabel
        /// </summary>
        /// <param name="label"></param>
        public void equipLabel(Label label)
        {
            // Check if equippedLabel is set
            if(equippedLabel != null)
            {
                equippedLabel.isEquipped = false;
                // Place equippedLabel back in inventory
                inventory.AddInventoryObject(equippedLabel);
            }
            // Set input label as equippedLabel
            equippedLabel = label;
            // Remove label from inventory as it is equipped
            inventory.RemInventoryObject(label);
        
            label.isEquipped = true;
        }

        /// <summary>
        /// Swap or set equippedCap
        /// </summary>
        /// <param name="cap"></param>
        public void equipCap(Cap cap)
        {
            // Check if equippedLabel is set
            if (equippedCap != null)
            {
                equippedCap.isEquipped = false;
                // Place equippedCap back in inventory
                inventory.AddInventoryObject(equippedCap);
            }
            // Set input cap as equippedCap
            equippedCap = cap;
            // Remove cap from inventory as it is equipped
            inventory.RemInventoryObject(cap);
            cap.isEquipped = true;
        }

    }
}
