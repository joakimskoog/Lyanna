namespace Lyanna.Hotkeys
{
    public interface IHotkeyManager
    {
        void AddHotkey(Hotkey hotkey, string name);

        void RemoveHotkey(string name);

        void RemoveAllHotkeys();
    }
}