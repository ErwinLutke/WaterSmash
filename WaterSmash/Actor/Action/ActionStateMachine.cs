using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Water
{
    class ActionStateMachine
    {
        Dictionary<string, IAction> _actionDict = new Dictionary<string, IAction>(); // Holds available actions

        public IAction currentAction; // Holds current action

        public void Add(string name, IAction action)
        {
            _actionDict.Add(name, action); 
        }

        /// <summary>
        /// Change current action
        /// </summary>
        /// <param name="name"></param>
        public void Change(string name)
        {
            currentAction = _actionDict[name];
        }

        // Update current action according to keys pressed
        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right))
            {
                Change("move");
            }
            //else if (state.IsKeyDown(Keys.Space))
            //{
            //    Change("jump");
            //}
            else
            {
                Change("stand");
            }

            currentAction.Update(gameTime);
        }

        public void HandleInput()
        {

        }
    }
}