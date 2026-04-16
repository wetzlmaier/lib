using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Scch.Common.Windows.Input
{
    /// <summary>
    /// Provides a global hook for <see cref="KeyGesture"/>.
    /// </summary>
    public class KeyboardHook : Disposable
    {
        /// <summary>
        /// Occurs when a <see cref="KeyGesture"/> is pressed.
        /// </summary>
        public event KeyGesturePressedEventHandler KeyGesturePressed;

        private readonly User32.LowLevelKeyboardProc _keyboardProc;
        private readonly IntPtr _hookId = IntPtr.Zero;
        private readonly IDictionary<EquatableKeyGesture, ICommand> _keyGestures;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardHook"/> class.
        /// </summary>
        public KeyboardHook()
        {
            _keyGestures = new Dictionary<EquatableKeyGesture, ICommand>();
            _keyboardProc = HookCallback;
            _hookId = SetHook(_keyboardProc);
        }

        /// <summary>
        /// <see cref="Disposable.Dispose(bool)"/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
                User32.UnhookWindowsHookEx(_hookId);

            base.Dispose(disposing);
        }

        /// <summary>
        /// Registers a <see cref="KeyGesture"/> to be hooked.
        /// </summary>
        /// <param name="gesture">The <see cref="KeyGesture"/>.</param>
        public void RegisterKeyGesture(KeyGesture gesture)
        {
            if (gesture == null)
                throw new ArgumentNullException("gesture");

            RegisterKeyGestureInternal(gesture, null);
        }

        /// <summary>
        /// Registers a <see cref="KeyGesture"/> to execute the specified <see cref="ICommand"/>.
        /// </summary>
        /// <param name="gesture">The <see cref="KeyGesture"/>.</param>
        /// <param name="command">The <see cref="ICommand"/>.</param>
        public void RegisterKeyGesture(KeyGesture gesture, ICommand command)
        {
            if (gesture == null)
                throw new ArgumentNullException("gesture");

            if (command == null)
                throw new ArgumentNullException("command");
            
            RegisterKeyGestureInternal(gesture, command);
        }

        private void RegisterKeyGestureInternal(KeyGesture gesture, ICommand command)
        {
            var newGesture = new EquatableKeyGesture(gesture);
            if (_keyGestures.ContainsKey(newGesture))
                throw new InvalidOperationException("KeyGesture already registered.");

            _keyGestures.Add(newGesture, command);
        }

        /// <summary>
        /// Unregisters a <see cref="KeyGesture"/> to be hooked.
        /// </summary>
        /// <param name="gesture"></param>
        public void UnregisterKeyGesture(KeyGesture gesture)
        {
            if (gesture == null)
                throw new ArgumentNullException("gesture");

            var newGesture = new EquatableKeyGesture(gesture);
            if (!_keyGestures.ContainsKey(newGesture))
                throw new InvalidOperationException("KeyGesture not registered.");

            _keyGestures.Remove(newGesture);
        }

        /// <summary>
        /// Raises the <see cref="KeyGesturePressed"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnKeyGesturePressed(KeyGesturePressedEventArgs e)
        {
            KeyGesturePressedEventHandler handler = KeyGesturePressed;
            if (handler != null) 
                handler(null, e);
        }

        private IntPtr SetHook(User32.LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return User32.SetWindowsHookEx(User32.HookConst.WH_KEYBOARD_LL, proc,
                                                   User32.GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WindowsMessages.WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                var keyPressed = KeyInterop.KeyFromVirtualKey(vkCode);

                if (EquatableKeyGesture.IsValid(keyPressed, Keyboard.Modifiers))
                {
                    var gesture = new EquatableKeyGesture(keyPressed, Keyboard.Modifiers);

                    ICommand command;
                    if (_keyGestures.TryGetValue(gesture, out command))
                    {
                        if (command != null)
                        {
                            if (command.CanExecute(null))
                                command.Execute(null);
                        }
                        else
                            OnKeyGesturePressed(new KeyGesturePressedEventArgs(gesture));
                    }
                }
            }

            return User32.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
    }
}
