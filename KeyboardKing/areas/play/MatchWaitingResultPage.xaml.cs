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
using System.Windows.Shapes;

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for MatchWaitingResultPage.xaml
    /// </summary>
    public partial class MatchWaitingResultPage : JumpPage
    {
        public MatchWaitingResultPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            int user_id = ((UList)Session.Get("student")).Get<int>(0);
            int match_id = MatchController.GetMatchId();
            DBQueries.UpdateMatchProgress(100, user_id, match_id);
        }

        public override void OnShadow()
        {

        }

        public override void OnTick()
        {
            Dispatcher.Invoke(() =>
            {
                if (MatchController.CheckIfEverybodyDone())
            {
                NavigationController.NavigateToPage(Pages.MatchResultPage);
            }
            });
        }
    }
}