using System;

namespace Lyanna.Hotkeys
{
    internal static class KeyExtensions
    {
        public static int ToVirtualKeyCode(this Key key)
        {
            switch (key)
            {
                case Key.Left: return 0x25;
                case Key.Right: return 0x27;
                case Key.Space: return 0x20;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}