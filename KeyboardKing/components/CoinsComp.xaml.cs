using Controller;
using Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KeyboardKing.components
{
    /// <summary>
    /// Interaction logic for Coin.xaml
    /// </summary>
    
    public partial class CoinsComp : UserControl
    {
        public event EventHandler Load;
        public Pages CurrentPage2 { get; set; } = Pages.Empty;

        public CoinsComp()
        {
            InitializeComponent();
            CoinsChange();
        }

        private void UserControl1_Load(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the UserControl.Load event.");
        }

        public void CoinsChange()
        {
            if(CurrentPage2 == Pages.ChaptersPage)
            {
                Coins.Content = EpisodeController.GetCoins((User)Session.Get("student"));
            }else
            {
                Coins.Content = "2";
            }
        }
    }
}
