using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KeyboardKing.core
{
    public class JumpPage : Page
    {
        /// <summary>
        /// Parent Window that contains the Frame.
        /// </summary>
        private MainWindow _window {get;set;}

        public JumpPage(MainWindow w)
        {
            _window = w;
        }

        /// <summary>
        /// Use the parent window to navigate by name.
        /// </summary>
        public void Navigate(string pageName)
        {
            _window.Navigate(pageName);
        }

        /// <summary>
        /// Use the parent window to navigate by button tag.
        /// Tag="TargetPageName" Click="ButtonNavigate"
        /// </summary>
        public void ButtonNavigate(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                _window.Navigate("" + ((Button)sender).Tag);
            }
        }
    }
}
