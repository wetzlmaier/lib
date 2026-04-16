using System;
using System.Windows.Input;

namespace Scch.Common.Windows.Input
{
    public class KeyGesturePressedEventArgs : EventArgs
    {
        public KeyGesturePressedEventArgs(KeyGesture gesture)
        {
            Gesture = gesture;
        }

        public KeyGesture Gesture { get; private set; }
    }
}
