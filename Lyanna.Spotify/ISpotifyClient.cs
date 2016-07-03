namespace Lyanna.Spotify
{
    /// <summary>
    /// Service that communicates with the Spotify client.
    /// </summary>
    public interface ISpotifyClient
    {
        /// <summary>
        /// Changes to the next track.
        /// </summary>
        void NextTrack();

        /// <summary>
        /// Changes to the previous track.
        /// </summary>
        void PreviousTrack();

        /// <summary>
        /// Either starts playing a track or pauses the currently playing track.
        /// </summary>
        void ToggleTrack();
    }
}