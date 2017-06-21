using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    /// <summary>
    /// Assists in locking keys to prevent multiple presses when held down
    /// </summary>
    public class KeyLocker
    {
        /// <summary>
        /// Holds the key that has been pressed
        /// </summary>
        public Keys PressedKey { get; set; }

        /// <summary>
        /// Used to check if a key has been pressed
        /// </summary>
        public bool KeyPressed { get; set; }

        /// <summary>
        /// Holds the previous keyboardstate. used to check if a key is pressed or not
        /// </summary>
        KeyboardState oldState;


        public KeyLocker()
        {
            KeyPressed = false;
        }

        /// <summary>
        /// Locks the specified key so that it can only be pressed once
        /// </summary>
        /// <param name="key">The key to lock</param>
        public void LockKey(Keys key)
        {
            PressedKey = key;
            KeyPressed = true;
        }

        /// <summary>
        /// Checks if the key is no longer being pressed and releases its lock
        /// </summary>
        /// <param name="state">The current keyboardstate to check against the key</param>
        /// <param name="key">The key to check</param>
        public void CheckInputLock(KeyboardState state, Keys key)
        {
            if (KeyPressed && PressedKey == key && !state.IsKeyDown(key) && !oldState.IsKeyDown(key))
            {
                KeyPressed = false;
            }


            // set the oldstate as the current state to prepare for the next state
            oldState = state;
        }
    }
}
