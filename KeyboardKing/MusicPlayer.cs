using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Timers;

namespace KeyboardKing
{
    public static class MusicPlayer
    {
        public static Dictionary<string, Dictionary<string, int>> Playlists {get;set;} = new Dictionary<string, Dictionary<string, int>>() {
            {
                "menu_music",
                new Dictionary<string, int>()
                {
                    {"Coffee", 125},
                    {"Fiber", 145},
                    {"Page", 94},
                    {"Riverside", 161},
                    {"Sunday", 163},
                }
            }
        };

        public static SoundPlayer Player {get;set;} = new SoundPlayer();

        private static Timer _timer {get;set;}

        public static Random Random = new Random();

        public static int Seconds {get;set;}

        public static string CurrentSong {get;set;} = "";

        public static string CurrentPlaylist {get;set;} = Playlists.Keys.First();

        public static int Index {get;set;} = Random.Next(0, Playlists["menu_music"].Count-1);

        public static void Tick(object sender, EventArgs e)
        {
            if (Seconds>=Playlists[CurrentPlaylist][CurrentSong])
            {
                Seconds = 0;
                PlayNext();
            } else
            {
                Seconds++;
            }
        }

        public static void NextIndex()
        {
            if ((Index + 1) == Playlists[CurrentPlaylist].Count)
            {
                Index = 0;
            } else
            {
                Index++;
            }
        }

        public static void PlayNext()
        {
            NextIndex();

            _timer?.Stop();
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(Tick);
            _timer.Interval = 1000;
            _timer.Start();

            CurrentSong = Playlists[CurrentPlaylist].Keys.ElementAt(Index);

            Player = new SoundPlayer($@"./resources/audio/music/{CurrentSong}.wav");
            Player.Play();
        }

        public static void Stop()
        {
            Player.Stop();
        }

        public static void Start()
        {
            Player.Play();
        }

        public static void PlayNextFrom(string playlist_name)
        {
            CurrentPlaylist = playlist_name;
            PlayNext();
        }
    }
}
