using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Interop;

namespace Lyanna.Hotkeys
{
    public class DefaultHotkeyManager : IHotkeyManager
    {
        private readonly IDictionary<string, Hotkey> _hotkeys;

        public DefaultHotkeyManager()
        {
            _hotkeys = new ConcurrentDictionary<string, Hotkey>();
        }

        public void AddHotkey(Hotkey hotkey, string name)
        {
            RemoveHotkey(name);
            _hotkeys.Add(name, hotkey);
        }

        public void RemoveHotkey(string name)
        {
            if (_hotkeys.ContainsKey(name))
            {
                var hotkeyToRemove = _hotkeys[name];
                hotkeyToRemove.Dispose();
                _hotkeys.Remove(name);
            }
        }

        public void RemoveAllHotkeys()
        {
            foreach (var name in _hotkeys.Keys)
            {
                RemoveHotkey(name);
            }
        }
    }
}