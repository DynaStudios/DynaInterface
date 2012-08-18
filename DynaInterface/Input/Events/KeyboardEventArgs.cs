using System;
using Microsoft.Xna.Framework.Input;

namespace DynaStudios.UI.Input.Events
{
    public class KeyboardEventArgs : EventArgs
    {
        private readonly Keys _pressedKey;

        public Keys PressedKey
        {
            get { return _pressedKey; }
        }

        public KeyboardEventArgs(Keys key)
        {
            _pressedKey = key;
        }
    }
}