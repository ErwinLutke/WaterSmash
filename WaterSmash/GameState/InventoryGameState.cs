using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Water
{
    class InventoryGameState : IGameState
    {
        private GameStateManager gameStateManager;
        private GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();


        SpriteBatch spriteBatch;
        Texture2D bg;       // Holds InverntoryGameState background
        Texture2D bottle;   // Holds bottle texture image

        AActor player = new Player(); // Player instance used for testing purposes

        private AInventoryObject current;

        private KeyboardState oldState;     // Holds old KeyBoardState

        private Boolean showCaps = true;
        private Boolean showLabels = false;

        public InventoryGameState(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
            spriteBatch = new SpriteBatch(graphics);
        }

        /// <summary>
        /// Draw inventory screen
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //Draw background full screen
            spriteBatch.Draw(bg, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White);

            //Draw bottle image
            spriteBatch.Draw(bottle, new Rectangle((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 100) * 10, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2, bottle.Width, bottle.Height), Color.White);

            // Used for layout inventory slots
            int count = 0;

            // Used for displaying items in inventory
            int itemPointer = 0;

            // y position placement for inventory slots
            int y = 40;
            foreach(Texture2D slot in player.GetInventory().slots)
            {
                // When 6 inventory slots are placed, start new line
                if(count == 6)
                {
                    y += slot.Height;
                    count = 0;
                }

                // Hold x position of current inventory slot
                int x = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) + (count * slot.Width);

                // Draw inventory slot image
                spriteBatch.Draw(slot, new Rectangle(x, y, slot.Width, slot.Height), Color.White);

                // Check for items in items list
                if(itemPointer < player.GetInventory().items.Count)
                {
                    if (player.GetInventory().items[itemPointer].isSelected)
                    {
                        current = player.GetInventory().items[itemPointer];
                        //Texture2D selection = content.Load<Texture2D>("inventory\\selection");
                        //spriteBatch.Draw(selection, new Rectangle(x + 10, y + 10, selection.Width, selection.Height), Color.White);
                        // Draw item image
                        spriteBatch.Draw(player.GetInventory().items[itemPointer].texture, new Rectangle(x + 10, y + 10, player.GetInventory().items[count].texture.Width + 20, player.GetInventory().items[count].texture.Height + 20), Color.White);
                        player.GetInventory().items[itemPointer].position = new Vector2(x + 10, y + 10);
                    }
                    else
                    {
                        // Draw item image
                        spriteBatch.Draw(player.GetInventory().items[itemPointer].texture, new Rectangle(x + 10, y + 10, player.GetInventory().items[count].texture.Width, player.GetInventory().items[count].texture.Height), Color.White);
                        player.GetInventory().items[itemPointer].position = new Vector2(x + 10, y + 10);
                    }
                    // Increment counter to get next item
                    itemPointer++;
                }
                // Increment counter for position purposes
                count++;
            }
            
            spriteBatch.End();
        }

        public void Entered(params object[] args)
        {
            // Get background image
            bg = content.Load<Texture2D>("inventory\\bg");
            // Get bottle image 
            bottle = content.Load<Texture2D>("inventory\\bottle");

            if(player.GetInventory().items.Count != 0)
            {
                player.GetInventory().items[0].isSelected = true;
            }        
        }

        public void HandleInput(KeyboardState state)
        {
            
        }

        public void Leaving()
        {
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            int index = player.GetInventory().items.IndexOf(current);

            if (oldState.IsKeyUp(Keys.Right) && newState.IsKeyDown(Keys.Right))
            {
                if (index + 1 < player.GetInventory().items.Count)
                {
                    current.isSelected = false;
                    current = player.GetInventory().items[index + 1];
                    current.isSelected = true;
                    Draw(gameTime);
                }
            }
            if (oldState.IsKeyUp(Keys.Left) && newState.IsKeyDown(Keys.Left))
            {
                if (index - 1 >= 0)
                {
                    current.isSelected = false;
                    current = player.GetInventory().items[index - 1];
                    current.isSelected = true;
                    Draw(gameTime);
                }
            }
            oldState = newState;
        }
    }
}
