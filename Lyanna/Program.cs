using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Lyanna.Hotkeys;
using Lyanna.Spotify;

namespace Lyanna
{
    class Program
    {
        private readonly ISpotifyClient _client;
        private readonly IHotkeyManager _hotkeyManager;

        public Program(ISpotifyClient client, IHotkeyManager hotkeyManager)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (hotkeyManager == null) throw new ArgumentNullException(nameof(hotkeyManager));
            _client = client;
            _hotkeyManager = hotkeyManager;

            _hotkeyManager.AddHotkey(new Hotkey(Key.Left, KeyModifier.Ctrl, hotkey => _client.PreviousTrack()), "PreviousTrack");
            _hotkeyManager.AddHotkey(new Hotkey(Key.Right, KeyModifier.Ctrl, hotkey => _client.NextTrack()), "NextTrack");
            _hotkeyManager.AddHotkey(new Hotkey(Key.Space, KeyModifier.Ctrl, hotkey => _client.ToggleTrack()), "ToggleTrack");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Lyanna - A CLI for global shortcuts for the Spotify client. (-h for help)");

            if (args.Any(s => s.Equals("-h")))
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("CTRL+Left to switch to the previous track");
                Console.WriteLine("CTRL+Right to switch to the next track");
                Console.WriteLine("CTRL+Space to pause/play the current track.");
                Console.WriteLine("CTRL+C to quit");
            }

            var hotkeyManager = new DefaultHotkeyManager();

            new Program(new UnmanagedSpotifyClient(), hotkeyManager);
            var lyannaApplication = new LyannaApplication();
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs eventArgs)
            {
                hotkeyManager.RemoveAllHotkeys();
                lyannaApplication.Shutdown();
            };

            //Ugly hack to make our console application run without closing and without doing stuff like Thread.Sleeps
            //or blocking the thread waiting for input.
            lyannaApplication.Run();
        }
    }

    public partial class LyannaApplication : Application
    {
    }
}