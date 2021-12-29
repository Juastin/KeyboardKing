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
        public CoinsComp()
        {
            InitializeComponent();
            Session.PropertyChanged += OnCoinsChange;
        }

        public void OnCoinsChange(object sender, EventArgs e)
        {
            User user = (User)Session.Get("student");
            if (user != null)
                Dispatcher.Invoke(() =>
                {
                    Coins.Content = user.Coins;
                });
        }
    }
}