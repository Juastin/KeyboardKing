﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Timers;

namespace KeyboardKing
{
    public static class MusicPlayer
    {
        private static bool _shouldplay {get;set;} = true;
        public static bool ShouldPlay
        {
            get
            {
                return _shouldplay;
            }
            set
            {
                _shouldplay = value;
                if (!value) {Stop();}
            }

        }

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
            },
            {
                "intense_music",
                new Dictionary<string, int>()
                {
                    {"There Was A King", 157}
                }
            }
        };

        public static SoundPlayer Player {get;set;} = new SoundPlayer();

        private static Timer _timer {get;set;}

        public static Random Random = new Random();

        public static double Seconds {get;set;}

        public static string CurrentSong {get;set;} = "";

        public static string CurrentPlaylist {get;set;} = Playlists.Keys.First();

        public static int Index {get;set;} = Random.Next(0, Playlists[Playlists.First().Key].Count-1);

        public static void Tick(object sender, EventArgs e)
        {
            if (Seconds>=Playlists[CurrentPlaylist][CurrentSong])
            {
                Seconds = 0;
                PlayNext();
            } else
            {
                Seconds += 0.100;
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

        public static void RandomizeIndex()
        {
            Index = Random.Next(0, Playlists[CurrentPlaylist].Count-1);
        }

        public static void PlayNext()
        {
            if (ShouldPlay)
            {
                NextIndex();

                _timer?.Stop();
                _timer = new Timer();
                _timer.Elapsed += new ElapsedEventHandler(Tick);
                _timer.Interval = 100;
                _timer.Start();

                CurrentSong = Playlists[CurrentPlaylist].Keys.ElementAt(Index);

                Player = new SoundPlayer($@"./resources/audio/music/{CurrentSong}.wav");
                Player.Play();
            }
        }

        public static void PlayNextFrom(string playlist_name)
        {
            CurrentPlaylist = playlist_name;
            RandomizeIndex();
            PlayNext();
        }

        public static void Stop()
        {
            Player.Stop();
        }

        public static void Start()
        {
            Player.Play();
        }
    }
}