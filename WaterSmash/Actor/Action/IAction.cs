using Microsoft.Xna.Framework;

namespace Water
{
    interface IAction
    {
        void Update(GameTime gameTime);
        void HandleInput();
        void Entered(params object[] args);
        void Leaving();
    }
}