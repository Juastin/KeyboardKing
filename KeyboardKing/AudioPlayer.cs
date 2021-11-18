using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardKing
{
    public static class AudioPlayer
    {
        public static bool ShouldPlay {get;set;} = false;

        public enum Sound
        {
            click,
            congratulations,
            failure
        }

        public static void Play(Sound s)
        {
            if (ShouldPlay)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer($@"./audio/{s}.wav");
                player.Play();
            }
        }
    }
}
