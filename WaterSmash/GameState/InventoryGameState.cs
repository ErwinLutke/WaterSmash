using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Water
{
    class InventoryGameState : IGameState
    {
        private GameStateManager gameStateManager;
        private GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        private ContentManager content = GameServices.GetService<ContentManager>();


        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D bg;       // Holds InverntoryGameState background
        Texture2D bottle;   // Holds bottle texture image
        Texture2D textArea; // Holds textarea
        Texture2D slot;

        //Player player = new Player(); // Player instance used for testing purposes
        Player player;

        Inventory inventory;

        private AEquipable current; // Holds current selected item

        private KeyboardState oldState;     // Holds old KeyBoardState

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

            Viewport viewport = graphics.Viewport;

            //Draw background full screen
            spriteBatch.Draw(bg, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);

            //Draw instruction text for equipping and dropping items
            spriteBatch.DrawString(spriteFont, "Press 'F' to equip", new Vector2(viewport.Width / 100 * 10, viewport.Height / 100 * 5), Color.White);
            spriteBatch.DrawString(spriteFont, "Press 'Del' to drop", new Vector2(viewport.Width / 100 * 10, viewport.Height / 100 * 10), Color.White);

            //Draw bottle image
            spriteBatch.Draw(bottle, new Rectangle((viewport.Width / 100) * 5, (viewport.Height / 100) * 30, bottle.Width, bottle.Height), Color.White);

            //Draw textareas for displaying information about current equipables
            spriteBatch.Draw(textArea, new Rectangle((viewport.Width / 100) * 5, (viewport.Height / 100) * 50, textArea.Width / 2, textArea.Height / 2 + (textArea.Height / 100) * 50), Color.White);
            spriteBatch.Draw(textArea, new Rectangle((viewport.Width / 100) * 25, (viewport.Height / 100) * 50, textArea.Width / 2, textArea.Height / 2 + (textArea.Height / 100) * 50), Color.White);

            //Draw title text for equipped items
            spriteBatch.DrawString(spriteFont, "Current equipped cap: ", new Vector2((viewport.Width / 100) * 5 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 2), Color.Black);
            spriteBatch.DrawString(spriteFont, "Current equipped label: ", new Vector2((viewport.Width / 100) * 25 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 2), Color.Black);

            //Check if player currently has a cap equipped
            if (player.equippedCap != null)
            {
                //Draw information about current equipped cap
                spriteBatch.DrawString(spriteFont, player.equippedCap.name, new Vector2((viewport.Width / 100) * 5 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 4), Color.Black);
                spriteBatch.DrawString(spriteFont, "Level: " + player.equippedCap.level, new Vector2((viewport.Width / 100) * 5 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 6), Color.Black);
                spriteBatch.DrawString(spriteFont, "Attack: " + player.equippedCap.attack, new Vector2((viewport.Width / 100) * 5 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 8), Color.Black);
                spriteBatch.DrawString(spriteFont, "Defense: " + player.equippedCap.defense, new Vector2((viewport.Width / 100) * 5 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 10), Color.Black);
                spriteBatch.Draw(player.equippedCap.texture, new Rectangle((viewport.Width / 100) * 15 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 6, player.equippedCap.texture.Width, player.equippedCap.texture.Height), Color.White);
            }
            //Check if player currently has a label equipped
            if (player.equippedLabel != null)
            {
                //Draw information about current equipped cap
                spriteBatch.DrawString(spriteFont, player.equippedLabel.name, new Vector2((viewport.Width / 100) * 25 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 4), Color.Black);
                spriteBatch.DrawString(spriteFont, "Level: " + player.equippedLabel.level, new Vector2((viewport.Width / 100) * 25 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 6), Color.Black);
                spriteBatch.DrawString(spriteFont, "Attack: " + player.equippedLabel.attack, new Vector2((viewport.Width / 100) * 25 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 8), Color.Black);
                spriteBatch.DrawString(spriteFont, "Defense: " + player.equippedLabel.defense, new Vector2((viewport.Width / 100) * 25 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 10), Color.Black);
                spriteBatch.Draw(player.equippedLabel.texture, new Rectangle((viewport.Width / 100) * 35 + (viewport.Width / 100) * 2, (viewport.Height / 100) * 50 + (viewport.Width / 100) * 6, player.equippedLabel.texture.Width, player.equippedLabel.texture.Height), Color.White);

            }


            // Used for layout inventory slots
            int count = 0;

            // Used for displaying items in inventory
            int itemPointer = 0;

            // y position placement for inventory slots
            int y = 40;

            
            foreach (Texture2D t in inventory.slots)
            {
                // When 6 inventory slots are placed, start new line
                if (count == 6)
                {
                    y += slot.Height;
                    count = 0;
                }

                // Hold x position of current inventory slot
                int x = (viewport.Width / 2) + (count * slot.Width);

                // Draw inventory slot image
                spriteBatch.Draw(slot, new Rectangle(x, y, slot.Width, slot.Height), Color.White);

                // Check for items in items list
                if (itemPointer < inventory.items.Count)
                {
                    // Check if item to draw is equipped -> items that are equipped are out of inventory, so not drawn here
                    if (!inventory.items[itemPointer].isEquipped)
                    {
                        // Check if item is selected
                        if (inventory.items[itemPointer].isSelected)
                        {
                            // Set this item as current.
                            current = inventory.items[itemPointer];
                            // Draw item image with selection (Slightly bigger)
                            spriteBatch.Draw(inventory.items[itemPointer].texture, new Rectangle(x + 10, y + 10, inventory.items[count].texture.Width + 20, inventory.items[count].texture.Height + 20), Color.White);
                            // Save on screen location of item
                            inventory.items[itemPointer].position = new Vector2(x + 10, y + 10);
                        }
                        else
                        {
                            // Draw item image
                            spriteBatch.Draw(inventory.items[itemPointer].texture, new Rectangle(x + 10, y + 10, inventory.items[count].texture.Width, inventory.items[count].texture.Height), Color.White);
                            // Save on screen location of item
                            inventory.items[itemPointer].position = new Vector2(x + 10, y + 10);
                        }
                    }
                    // Increment counter to get next item
                    itemPointer++;
                }
                // Increment counter for position purposes
                count++;
            }

            // Draw textarea to display current information about selected item
            spriteBatch.Draw(textArea, new Rectangle((viewport.Width / 2), inventory.slots[0].Height * 6, inventory.slots[0].Width * 6, textArea.Height), Color.White);

            // Draw information about current selected item
            spriteBatch.DrawString(spriteFont, current.name, new Vector2((viewport.Width / 2) + (viewport.Width / 100 * 5), (inventory.slots[0].Height * 6) + (viewport.Width / 100 * 3)), Color.Black);
            spriteBatch.DrawString(spriteFont, "Level: " + current.level, new Vector2((viewport.Width / 2) + (viewport.Width / 100 * 5), (inventory.slots[0].Height * 6) + (viewport.Width / 100 * 5)), Color.Black);
            spriteBatch.DrawString(spriteFont, "Attack: " + current.attack, new Vector2((viewport.Width / 2) + (viewport.Width / 100 * 5), (inventory.slots[0].Height * 6) + (viewport.Width / 100 * 7)), Color.Black);
            spriteBatch.DrawString(spriteFont, "Defense: " + current.defense, new Vector2((viewport.Width / 2) + (viewport.Width / 100 * 5), (inventory.slots[0].Height * 6) + (viewport.Width / 100 * 9)), Color.Black);

            spriteBatch.End();
        }


        public void Entered(params object[] args)
        {
            if (player == null)
            {
                if (args.Length > 0)
                {
                    player = (Player)args[0];
                }
                else
                {
                    player = new Player();
                }
            }

            inventory = player.GetInventory();

            // Get background image
            bg = content.Load<Texture2D>("inventory\\bg");
            // Get bottle image 
            bottle = content.Load<Texture2D>("inventory\\bottle");
            // Get textarea image
            textArea = content.Load<Texture2D>("inventory\\inventory_tekst");
            // Load spritefont
            spriteFont = content.Load<SpriteFont>("inventory\\inventory");
            
            slot = content.Load<Texture2D>("inventory\\inventory_slot");

            // Set first item as selected item
            if (inventory.items.Count != 0)
            {
                inventory.items[0].isSelected = true;
            }
        }

        public void HandleInput(KeyboardState state)
        {
            // Get index of item set as current
            int index = inventory.items.IndexOf(current);

            // Check if right key is pressed
            if (oldState.IsKeyUp(Keys.Right) && state.IsKeyDown(Keys.Right))
            {
                nextItem(index);
            }
            // Check if left key is pressed
            else if (oldState.IsKeyUp(Keys.Left) && state.IsKeyDown(Keys.Left))
            {
                previousItem(index);
            }
            // Check if 'F' key is pressed
            else if (oldState.IsKeyUp(Keys.F) && state.IsKeyDown(Keys.F))
            {
                equipItem();
            }
            // Check if 'Delete' key is pressed
            else if (oldState.IsKeyUp(Keys.Delete) && state.IsKeyDown(Keys.Delete))
            {
                dropItem();
            }


            oldState = state;
        }

        public void Leaving()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {

        }

        private void nextItem(int index)
        {
            // Check if List which holds items has a next item after current
            if (index + 1 < inventory.items.Count)
            {
                // Deselect current item
                current.isSelected = false;
                // Set next item as new current item
                current = inventory.items[index + 1];
                // Set select new current item
                current.isSelected = true;
            }
        }

        private void previousItem(int index)
        {
            // Check if List which holds items has an item before current
            if (index - 1 >= 0)
            {
                // Deselect current item
                current.isSelected = false;
                // Set previous item as new current item
                current = inventory.items[index - 1];
                // Set select new current item
                current.isSelected = true;

            }
        }

        private void dropItem()
        {
            // Check if current not is null
            if (current != null)
            {
                // Remove item from inventory
                inventory.RemInventoryObject(current);
            }
        }

        private void equipItem()
        {
            // Check if current not is null
            if (current != null)
            {
                // Check if current is instance of Cap
                if (current is Cap)
                {
                    // Equip Cap
                    player.equipCap((Cap)current);
                }
                // Check if current is instance of Label
                else if (current is Label)
                {
                    // Equip Label
                    player.equipLabel((Label)current);
                }
            }
        }
    }
}
