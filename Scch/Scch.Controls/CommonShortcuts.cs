using System;
using System.Windows.Input;

namespace Scch.Controls
{
    public static class CommonShortcuts
    {
        public static KeyGesture CreateNewShortcut()
        {
            return new KeyGesture(Key.N, ModifierKeys.Control);
        }

        public static KeyGesture CreateAddNewShortcut()
        {
            return new KeyGesture(Key.A, ModifierKeys.Control);
        }

        public static KeyGesture CreateDeleteShortcut()
        {
            return new KeyGesture(Key.D, ModifierKeys.Control);
        }

        public static KeyGesture CreateOkShortcut()
        {
            return new KeyGesture(Key.O, ModifierKeys.Alt);
        }

        public static KeyGesture CreateCancelShortcut()
        {
            return new KeyGesture(Key.Escape);
        }

        public static KeyGesture CreateEditShortcut()
        {
            return new KeyGesture(Key.E, ModifierKeys.Control);
        }

        public static KeyGesture CreateRenameShortcut()
        {
            return new KeyGesture(Key.R, ModifierKeys.Control);
        }

        public static KeyGesture CreateRedoShortcut()
        {
            return new KeyGesture(Key.Y, ModifierKeys.Control);
        }

        public static KeyGesture CreateUndoShortcut()
        {
            return new KeyGesture(Key.Z, ModifierKeys.Control);
        }

        public static KeyGesture CreateRefreshShortcut()
        {
            return new KeyGesture(Key.F5);
        }

        public static KeyGesture CreateZoomInShortcut()
        {
            return new KeyGesture(Key.OemPlus, ModifierKeys.Control);
        }

        public static KeyGesture CreateZoomOutShortcut()
        {
            return new KeyGesture(Key.OemMinus, ModifierKeys.Control);
        }

        public static KeyGesture CreateCopyShortcut()
        {
            return new KeyGesture(Key.C, ModifierKeys.Control);
        }

        public static KeyGesture CreateCutShortcut()
        {
            return new KeyGesture(Key.X, ModifierKeys.Control);
        }

        public static KeyGesture CreatePasteShortcut()
        {
            return new KeyGesture(Key.V, ModifierKeys.Control);
        }

        public static KeyGesture CreateFindShortcut()
        {
            return new KeyGesture(Key.F, ModifierKeys.Control);
        }

        public static KeyGesture CreateSearchShortcut()
        {
            return new KeyGesture(Key.F, ModifierKeys.Control);
        }

        public static KeyGesture CreateForwardMailShortcut()
        {
            return new KeyGesture(Key.F, ModifierKeys.Control);
        }

        public static KeyGesture CreateNewMailShortcut()
        {
            return new KeyGesture(Key.N, ModifierKeys.Control);
        }

        public static KeyGesture CreateHelpShortcut()
        {
            return new KeyGesture(Key.F1);
        }

        public static KeyGesture CreateOpenShortcut()
        {
            return new KeyGesture(Key.O, ModifierKeys.Control);
        }

        public static KeyGesture CreateSaveShortcut()
        {
            return new KeyGesture(Key.S, ModifierKeys.Control);
        }

        public static KeyGesture CreatePreviewShortcut()
        {
            return new KeyGesture(Key.F2, ModifierKeys.Control);
        }

        public static KeyGesture CreatePrintShortcut()
        {
            return new KeyGesture(Key.P, ModifierKeys.Control);
        }

        public static KeyGesture CreatePropertiesShortcut()
        {
            return new KeyGesture(Key.F4);
        }

        public static KeyGesture CreateNumberingShortcut()
        {
            return new KeyGesture(Key.L, ModifierKeys.Control | ModifierKeys.Alt);
        }

        public static KeyGesture CreateBulletsShortcut()
        {
            return new KeyGesture(Key.L, ModifierKeys.Control | ModifierKeys.Shift);
        }

        public static KeyGesture CreateMoveUpShortcut()
        {
            return new KeyGesture(Key.U, ModifierKeys.Control);
        }

        public static KeyGesture CreateMoveDownShortcut()
        {
            return new KeyGesture(Key.D, ModifierKeys.Control);
        }

        public static KeyGesture CreateRecordShortcut()
        {
            return new KeyGesture(Key.R, ModifierKeys.Control);
        }
    }
}
