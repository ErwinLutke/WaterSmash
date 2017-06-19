using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    interface IAction
    {
        void Entered(params object[] args);
        void HandleInput(KeyboardState state);
        void Update(GameTime gameTime);
        void Leaving();
    }
}