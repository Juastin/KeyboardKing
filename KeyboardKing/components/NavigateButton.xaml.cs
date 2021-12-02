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
using Controller;
using Model;

namespace KeyboardKing.components
{
    /// <summary>
    /// Interaction logic for NavigateButton.xaml
    /// </summary>
    public partial class NavigateButton : UserControl
    {
        public Pages ToPage { get; set; }
        public string Text { get; set; }

        public NavigateButton()
        {
            InitializeComponent();
        }

        private void Navigate_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigateController.NavigateToPage(ToPage);
        }
    }
}
