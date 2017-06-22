using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Water
{
    class ActionStateMachine
    {
        Dictionary<string, IAction> _actionDict = new Dictionary<string, IAction>(); // Holds available actions
        IAction currentAction = new EmptyAction();

        public IAction Current { get { return currentAction; } }
        public void Add(string name, IAction action)
        {
            _actionDict.Add(name, action); 
        }
        public void Remove(string name)
        {
            _actionDict.Remove(name);
        }
        public void Clear()
        {
            _actionDict.Clear();
        }

        /// <summary>
        /// Change current action
        /// </summary>
        /// <param name="name"></param>
        public void Change(string name, params object[] args)
        {
            currentAction.Leaving();
            IAction next = _actionDict[name];
            next.Entered(args);
            currentAction = next;
        }

        public void HandleInput(KeyboardState state)
        {
            currentAction.HandleInput(state);
        }

        // Update current action according to keys pressed
        public void Update(GameTime gameTime)
        {
            currentAction.Update(gameTime);
        }
    }
}