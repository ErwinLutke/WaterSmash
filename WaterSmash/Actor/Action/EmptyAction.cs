using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class EmptyAction : IAction
    {
        public void Entered(params object[] args)
        {
        }

        public void HandleInput(KeyboardState state)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Leaving()
        {
        }
    }
}
