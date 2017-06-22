using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    class GameStateManager
    {
        Dictionary<string, IGameState> _stateDict = new Dictionary<string, IGameState>();

        IGameState _current = new EmptyState();
        IGameState _previous;

        public IGameState Current { get { return _current; } }
        public IGameState Previous { get { return _previous; } }

        public void Add(string id, IGameState state) { _stateDict.Add(id, state); }
        public void Remove(string id) { _stateDict.Remove(id); }
        public void Clear() { _stateDict.Clear(); }


        public void Change(string id, params object[] args)
        {
            _current.Leaving();
            IGameState next = _stateDict[id];
            _previous = _current;
            next.Entered(args);
            _current = next;
        }

        public void Update(GameTime gameTime)
        {
            _current.Update(gameTime);
        }

        public void HandleInput(KeyboardState state)
        {
            _current.HandleInput(state);
        }

        public void Draw(GameTime gameTime)
        {
            _current.Draw(gameTime);
        }


    }
}