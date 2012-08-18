using System;

namespace DynaStudios.UI.Input.Events
{
    public class MouseEventArgs : EventArgs
    {
        public MouseKey PressedKey;

        public MouseEventArgs(MouseKey mouseKey)
        {
            PressedKey = mouseKey;
        }
    }
}