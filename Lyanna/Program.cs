using System;
using Lyanna.Spotify;
using Lyanna.Spotify.Properties;

namespace Lyanna
{
    class Program
    {
        private readonly ISpotifyClient _client;

        public Program(ISpotifyClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            _client = client;
        }

        public void Run()
        {
            
        }

        static void Main(string[] args)
        {
            var app = new Program(new UnmanagedSpotifyClient());
            app.Run();
        }
    }
}
