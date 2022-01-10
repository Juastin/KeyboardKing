using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace KeyboardKing
{
    public static class AudioPlayer
    {
        /// <summary>
        /// Used to globally mute all audio.
        /// </summary>
        public static bool ShouldPlay {get;set;} = true;

        /// <summary>
        /// List of all sounds, reflects the files of KeyboardKing/resources/audio
        /// </summary>
        public enum Sound
        {
            congratulations,
            failure,
            shop_enter,
            shop_exit,
            shop_preview,
            shop_purchase,
        }

        /// <summary>
        /// Used to play sound, expects a Sound enum.
        /// </summary>
        public static void Play(Sound s)
        {
            if (ShouldPlay)
            {
                SoundPlayer player = new SoundPlayer($@"./resources/audio/{s}.wav");
                player.Play();
            }
        }
    }
}
