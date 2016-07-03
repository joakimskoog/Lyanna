using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Lyanna.Hotkeys
{
    public class Hotkey
    {
        private const int WmHotKey = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, UInt32 fsModifiers, UInt32 vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static Dictionary<int, Hotkey> _dictHotKeyToCallBackProc;
        private bool _disposed;

        public Key Key { get; }
        public KeyModifier KeyModifiers { get; }
        public Action<Hotkey> Action { get; }
        public int Id { get; private set; }

        public Hotkey(Key k, KeyModifier keyModifiers, Action<Hotkey> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            Key = k;
            KeyModifiers = keyModifiers;
            Action = action;
            Register();
        }

        private void Register()
        {
            int virtualKeyCode = Key.ToVirtualKeyCode();
            Id = virtualKeyCode + ((int)KeyModifiers * 0x10000);
            RegisterHotKey(IntPtr.Zero, Id, (UInt32)KeyModifiers, (UInt32)virtualKeyCode);

            if (_dictHotKeyToCallBackProc == null)
            {
                _dictHotKeyToCallBackProc = new Dictionary<int, Hotkey>();
                ComponentDispatcher.ThreadFilterMessage += ComponentDispatcherThreadFilterMessage;
            }

            _dictHotKeyToCallBackProc.Add(Id, this);
        }

        private void Unregister()
        {
            Hotkey hotKey;
            if (_dictHotKeyToCallBackProc.TryGetValue(Id, out hotKey))
            {
                UnregisterHotKey(IntPtr.Zero, hotKey.Id);
            }
        }

        private static void ComponentDispatcherThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if (!handled)
            {
                if (msg.message == WmHotKey)
                {
                    Hotkey hotKey;

                    if (_dictHotKeyToCallBackProc.TryGetValue((int)msg.wParam, out hotKey))
                    {
                        hotKey.Action?.Invoke(hotKey);
                        handled = true;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Unregister();
                }

                _disposed = true;
            }
        }
    }
}