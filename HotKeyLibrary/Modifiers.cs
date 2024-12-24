using System;
using System.Linq;

namespace HotKeyLibrary
{
    [Flags]
    public enum Modifiers
    {
        LeftCtrl = 1,
        RightCtrl = 2,
        Ctrl = 4,
        LeftAlt = 8,
        RightAlt = 16,
        Alt = 32,
        LeftShift = 64,
        RightShift = 128,
        Shift = 256,
        All = LeftCtrl |
            RightCtrl |
            Ctrl |
            LeftAlt |
            RightAlt |
            Alt |
            LeftShift |
            RightShift |
            Shift,
        None = 0
    }
}
