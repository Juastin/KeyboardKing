using KeyboardKing.core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for EpisodePage.xaml
    /// </summary>
    public partial class EpisodePage : JumpPage
    {
        public EpisodePage(MainWindow w) : base(w)
        {
            InitializeComponent();
            this.UserInput.Focus();
        }

        public override void OnLoad()
        {
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void UserInput_LostFocus(object sender, RoutedEventArgs e)
        {
            this.UserInput.Focus();
        }

        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = this.UserInput.Text;
            char character = txt[^1];

            if (character != 'a')
                throw new Exception("wrong");
        }
    }
}
