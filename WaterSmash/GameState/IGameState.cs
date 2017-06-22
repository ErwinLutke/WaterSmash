using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    interface IGameState
    {
        // Called before entering the state
        void Entered(params object[] args);

        // Handles all player input
        void HandleInput(KeyboardState state);

        // Handles all game logic
        void Update(GameTime gameTime);

        // Handles drawing of textures and sprite
        void Draw(GameTime gameTime);

        // Called when changing state
        void Leaving();


        /* Following example illustrates when methods are called when changing states:
         * We have 2 states, state1 and state2
         *
         *  - CurrentState = state1
         * Currently we are in state1 and want to go to state2
         * 
         * - GameManager.Change(state2)
         * gameManager calls Leaving() of state1
         * gameManager changes CurrentState to state2
         * gameManager calls Entered() of state2
         * 
         * And then update, draw, handleinput will be called
         */
    }
}
