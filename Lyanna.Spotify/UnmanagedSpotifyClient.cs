using System;
using System.Runtime.InteropServices;

namespace Lyanna.Spotify
{
    public class UnmanagedSpotifyClient : ISpotifyClient
    {
        private const uint WmAppcommand = 0x0319;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public void NextTrack()
        {
            SendMessage(SpotifyAction.NextTrack);
        }

        public void PreviousTrack()
        {
            SendMessage(SpotifyAction.PreviousTrack);
        }

        public void ToggleTrack()
        {
            SendMessage(SpotifyAction.ToggleTrack);
        }

        private void SendMessage(SpotifyAction action)
        {
            var spotifyWindowHandle = GetSpotifyWindowHandle();
            if (spotifyWindowHandle == IntPtr.Zero) //We couldn't find the Spotify client
            {
                throw new Exception("Could not find the Spotify client, make sure it's running!");
            }

            SendMessage(spotifyWindowHandle, WmAppcommand, IntPtr.Zero, new IntPtr((long)action));
        }

        private IntPtr GetSpotifyWindowHandle()
        {
            return FindWindow("SpotifyMainWindow", null);
        }
    }
}