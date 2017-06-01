using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    interface IGameState
    {
        void Update(GameTime gameTime);
        void HandleInput(KeyboardState state);
        void Draw(GameTime gameTime);
        void Entered(params object[] args);
        void Leaving();
    }
}
