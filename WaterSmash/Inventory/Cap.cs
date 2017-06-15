using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Water
{
    class Cap : AEquipable
    {
        public Cap(int attack, int defense, int level, int grade, int special) : base(attack, defense, level, grade, special)
        {
            texture = content.Load<Texture2D>("inventory\\cap");
            name = "CAP";
        }
    }
}