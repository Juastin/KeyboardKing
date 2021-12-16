using Controller;
using Model;
using KeyboardKing.core;
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

namespace KeyboardKing.areas.main
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : JumpPage
    {
        public SettingsPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            Coin.Content = EpisodeController.GetCoins((User)Session.Get("student"));
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }
    }
}
