using Microsoft.Xna.Framework.Graphics;

namespace Water
{
    class Label : AEquipable
    {
        public Label(int attack, int defense, int level, int grade, int special) : base(attack, defense, level, grade, special)
        {
            texture = content.Load<Texture2D>("inventory\\lable");
        }
    }
}