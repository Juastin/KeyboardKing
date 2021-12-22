using Controller;
using KeyboardKing.core;
using System;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchResultPage.xaml
    /// </summary>
    public partial class MatchResultPage : JumpPage
    {
        private DateTime _tickCheck {get;set;} = DateTime.Now;

        /// <summary>
        /// Controller for <see cref="MatchResultPage"/>
        /// </summary>
        /// <param name="w"></param>
        public MatchResultPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            MusicPlayer.Stop();
            AudioPlayer.Play(AudioPlayer.Sound.congratulations);
            // Used by MatchHistoryPage to determine if a match has been played.
            Session.Add("MatchHasBeenPlayed", 13);
        }

        public override void OnShadow()
        {
            MusicPlayer.PlayNextFrom("menu_music");
        }

        public override void OnTick()
        {
            DateTime now = DateTime.Now;
            if (_tickCheck.AddSeconds(2) < now)
            {
                MatchController.SetWinners();
            }
        }
    }
}
