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
using KeyboardKing.data_context;

namespace KeyboardKing.components
{
    public class NavigationButton : Button
    {
        public Pages ToPage { get; set; } = Pages.Empty;

        static NavigationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationButton), new FrameworkPropertyMetadata(typeof(NavigationButton)));
        }

        protected override void OnClick()
        {
            NavigationController.NavigateToPage(ToPage);
        }
    }
}
