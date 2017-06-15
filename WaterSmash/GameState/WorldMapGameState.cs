using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Water
{
    [DataContract]
    class WorldMapGameState : IGameState
    {

        [DataMember]
        int stageProgress { get; set; }
        public GraphicsDevice GraphicsDevice { get; }

        List<string> _stageNames = new List<string>();
        AActor player;
        private GameStateManager gameStateManager;

        Texture2D image;
        // Create a new SpriteBatch, which can be used to draw textures.

        GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        ContentManager content = GameServices.GetService<ContentManager>();
        SpriteBatch spriteBatch;

        public WorldMapGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
            this.spriteBatch = new SpriteBatch(graphics);
        }

        public void Draw(GameTime gameTime) 
        {
            spriteBatch.Begin();

            spriteBatch.Draw(image, new Rectangle(0, 0, 800, 480), Color.White);

            spriteBatch.End();
        }

        public void Entered(params object[] args)
        {
            image = content.Load<Texture2D>("worldmap");
         //   player = (AActor)args[0];
        }

        public void HandleInput(KeyboardState state)
        {
        }

        public void Leaving()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
