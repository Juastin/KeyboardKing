using Controller;
using KeyboardKing.core;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchResultPage.xaml
    /// </summary>
    public partial class MatchResultPage : JumpPage
    {
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
            int user_id = ((UList)Session.Get("student")).Get<int>(0);
            int match_id = MatchController.GetMatchId();
            DBQueries.UpdateMatchProgress(100, user_id, match_id);

            MusicPlayer.Stop();
            AudioPlayer.Play(AudioPlayer.Sound.congratulations);
        }

        public override void OnShadow()
        {
            MusicPlayer.PlayNextFrom("menu_music");
        }

        public override void OnTick()
        {
        }
    }
}
