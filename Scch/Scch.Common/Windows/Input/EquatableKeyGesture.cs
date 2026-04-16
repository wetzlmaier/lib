using System;
using System.Windows.Input;

namespace Scch.Common.Windows.Input
{
    public class EquatableKeyGesture : KeyGesture, IEquatable<EquatableKeyGesture>
    {
        public EquatableKeyGesture(KeyGesture gesture)
            : base(gesture.Key, gesture.Modifiers, gesture.DisplayString)
        {
        }

        public EquatableKeyGesture(Key key, ModifierKeys modifiers)
            : base(key, modifiers)
        {
        }

        public bool Equals(EquatableKeyGesture other)
        {
            return (other != null && Key == other.Key && Modifiers == other.Modifiers);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj == null || obj.GetType() != GetType())
                return false;

            return Equals((EquatableKeyGesture)obj);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Modifiers.GetHashCode();
        }

        public static bool IsValid(Key key, ModifierKeys modifiers)
        {
            if (((key < Key.F1) || (key > Key.F24)) && ((key < Key.NumPad0) || (key > Key.Divide)))
            {
                if ((modifiers & (ModifierKeys.Windows | ModifierKeys.Control | ModifierKeys.Alt)) != ModifierKeys.None)
                {
                    switch (key)
                    {
                        case Key.LWin:
                        case Key.RWin:
                        case Key.LeftCtrl:
                        case Key.RightCtrl:
                        case Key.LeftAlt:
                        case Key.RightAlt:
                            return false;
                    }
                    return true;
                }
                if (((key >= Key.D0) && (key <= Key.D9)) || ((key >= Key.A) && (key <= Key.Z)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
