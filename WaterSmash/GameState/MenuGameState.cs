using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Water
{
    class MenuGameState : IGameState
    {
        private GameStateManager gameStateManager;
        GraphicsDevice graphics = GameServices.GetService<GraphicsDevice>();
        ContentManager content = GameServices.GetService<ContentManager>();
        SpriteBatch spriteBatch;
        Texture2D image;
        List<Button> buttons = new List<Button>();
        Button CurrentButton;
        int index = 0;




        public MenuGameState(GameStateManager gameStateManager)
        {
            
            addButtons();
            this.gameStateManager = gameStateManager;
            
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(image, new Rectangle(0, 0, 800, 480), Color.White);

            

            spriteBatch.End();
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
            //buttons.Draw(spriteBatch);
            //start.Update();
        }

        private void addButtons()
        {
            Viewport viewport = graphics.Viewport;

            Vector2 screenCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            

            Rectangle button = new Rectangle((viewport.Width / 2) - (306/2), 50, 306, 64);
            Texture2D play = content.Load<Texture2D>("play_default");
            Texture2D playHover = content.Load<Texture2D>("play_hover");
            buttons.Add(new Button(button, play, playHover, play, true));


            Texture2D newGame = content.Load<Texture2D>("play_default");
            Texture2D newGameHover = content.Load<Texture2D>("play_hover");
            buttons.Add(new Button(new Rectangle((viewport.Width / 2) - (306 / 2), 150, 306, 64), newGame, newGameHover, newGame, false));



            Texture2D loadGame = content.Load<Texture2D>("play_default");
            Texture2D loadGameHover = content.Load<Texture2D>("play_hover");
            buttons.Add(new Button(new Rectangle((viewport.Width / 2) - (306 / 2), 250, 306, 64), loadGame, loadGameHover, loadGame, false));



            Texture2D saveGame = content.Load<Texture2D>("play_default");
            Texture2D saveGameHover = content.Load<Texture2D>("play_hover");
            buttons.Add(new Button(new Rectangle((viewport.Width / 2) - (306 / 2), 350, 306, 64), saveGame, saveGameHover, saveGame, false));


            Texture2D quit = content.Load<Texture2D>("play_default");
            Texture2D quitHover = content.Load<Texture2D>("play_hover");
            buttons.Add(new Button(new Rectangle((viewport.Width / 2) - (306 / 2), 450, 306, 64), quit, quitHover, quit, false));
        }

        public void Entered(params object[] args)
        {
            CurrentButton = buttons[0];
            this.spriteBatch = new SpriteBatch(graphics);
            image = content.Load<Texture2D>("start");
            Texture2D play = content.Load<Texture2D>("play_default");
            Texture2D playHover = content.Load<Texture2D>("play_hover");
            
        }
        bool keylock = false;
        KeyboardState oldState;
        public void HandleInput(KeyboardState state)
        {
            if (!keylock)
            {
                if (state.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
                {
                    if (index < buttons.Count()-1)
                    {
                        index++;
                        CurrentButton.setSelected(false);
                        CurrentButton = buttons[index];
                        CurrentButton.setSelected(true);                       
                    }
                    else
                    {

                    }
                }
                if (state.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
                {
                    if (index > 0)
                    {
                        index--;
                        CurrentButton.setSelected(false);
                        CurrentButton = buttons[index];
                        CurrentButton.setSelected(true); 
                    }

                }
                if (state.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    if(CurrentButton == buttons[0])
                    {
                        gameStateManager.Change("worldmap", new Player());
                    }
                    if(CurrentButton == buttons[1])
                    {
                        gameStateManager.Change("worldmap");
                    }
                    if (CurrentButton == buttons[2])
                    {
                        App.Current.Exit();
                    }

                }
            }
            oldState = state;
            //wait();
        }

        
        public void Leaving()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var button in buttons)
            {
                button.Update();
            }
        }
    }
}
