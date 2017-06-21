using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Water
{
    internal class StandAction : IAction
    {
        private AActor _actor;
        private ActionStateMachine _actionStateMachine;

        public StandAction(AActor actor)
        {
            _actor = actor;
            _actionStateMachine = actor.actionStateMachine;
        }

        public void Entered(params object[] args)
        {

        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void HandleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Up)) 
            {
                _actionStateMachine.Change("jump");
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                _actionStateMachine.Change("crouch");
            }
            else if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right)) // If left or right key is pressed
            {
                _actionStateMachine.Change("move"); 
            }
            else if (state.IsKeyDown(Keys.Space))
            {
                _actionStateMachine.Change("throw");
            }
        }

        public void Leaving()
        {

        }
    }
}