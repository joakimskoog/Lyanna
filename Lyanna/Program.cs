using System;
using System.Linq;
using System.Windows;
using Lyanna.Hotkeys;
using Lyanna.Spotify;

namespace Lyanna
{
    class Program
    {
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
            var client = new UnmanagedSpotifyClient();

            hotkeyManager.AddHotkey(new Hotkey(Key.Left, KeyModifier.Ctrl, delegate
            {
                client.PreviousTrack();
                Console.WriteLine("Switching to previous track...");
            }), "PreviousTrack");
            hotkeyManager.AddHotkey(new Hotkey(Key.Right, KeyModifier.Ctrl, delegate
            {
                client.NextTrack();
                Console.WriteLine("Switching to next track...");
            }), "NextTrack");
            hotkeyManager.AddHotkey(new Hotkey(Key.Space, KeyModifier.Ctrl, delegate
            {
                client.ToggleTrack();
                Console.WriteLine("Starting/pausing current track...");
            }), "ToggleTrack");

            var lyannaApplication = new LyannaApplication();
            Console.CancelKeyPress += delegate
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