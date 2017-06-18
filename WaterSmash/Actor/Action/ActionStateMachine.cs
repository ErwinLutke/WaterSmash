using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Water
{
    class ActionStateMachine
    {
        Dictionary<string, IAction> _actionDict = new Dictionary<string, IAction>(); // Holds available actions

        public IAction currentAction; // Holds current action

        public AActor actor; // Holds current actor

        public ActionStateMachine(){}

        public ActionStateMachine(AActor actor)
        {
            this.actor = actor;
        }

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

            if (state.IsKeyDown(Keys.Space)) // If space is pressed
            {
                Change("jump"); // Change to JumpAction
            }
            else if(state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right)) // If left or right key is pressed
            {
                if(!actor.isJumping) // Check if actor is jumping, cannot change to move mid air
                {
                    Change("move"); // Change to MoveAction
                }                 
            }
            else
            {
                if (!actor.isJumping) // Check if actor is jumping, cannot change to move mid air
                {
                    Change("stand"); // Change to StandAction
                }
            }
            currentAction.Update(gameTime);
        }

        public void HandleInput()
        {

        }
    }
}