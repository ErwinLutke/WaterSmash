using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Water
{
    class Button
    {
        Song buttonSound;
        ContentManager content = GameServices.GetService<ContentManager>();
        public enum State
        {
            None,
            Pressed,
            Hover,
            Released
        }

        private Rectangle _rectangle;
        private State _state;
        private bool selected = false;
        public State States
        
        {
            get { return _state; }
            set { _state = value; } // you can throw some events here if you'd like
        }

        private Dictionary<State, Texture2D> _textures;

        public Button(Rectangle rectangle, Texture2D noneTexture, Texture2D hoverTexture, Texture2D pressedTexture, bool selected)
        {
            buttonSound = content.Load<Song>("audio/2nd_click_rear");
            _rectangle = rectangle;
            _textures = new Dictionary<State, Texture2D>
            
        {
            { State.None, noneTexture },
            { State.Hover, hoverTexture },
            { State.Pressed, pressedTexture }
        };
            this.selected = selected;
            _state = State.Hover;
        }

        public void Update()//KeyboardState state
        {

            if (selected)
            {
                
                _state = State.Hover;
            }
            else
            {
                
                _state = State.None;
            }
        }

        public void setSelected(bool selected)
        {
            this.selected = selected;
        }

        // Make sure Begin is called on s before you call this function
        public void Draw(SpriteBatch s)
        {
            s.Begin();  
            s.Draw(_textures[_state], _rectangle, Color.White);
            s.End();   
        }
        public void Play()
        {
            MediaPlayer.Play(buttonSound);
            MediaPlayer.IsRepeating = false;
        }

    }
}
