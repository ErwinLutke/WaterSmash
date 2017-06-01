using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;

namespace Water
{
    [DataContract]
    class WorldMapGameState : IGameState
    {

        [DataMember]
        int stageProgress { get; set; }

        List<string> _stageNames = new List<string>();
        AActor player;
        private GameStateManager gameStateManager;

        public WorldMapGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Entered(params object[] args)
        {
            player = (AActor)args[0];
            throw new NotImplementedException();
        }

        public void HandleInput(KeyboardState state)
        {
            throw new NotImplementedException();
        }

        public void Leaving()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
