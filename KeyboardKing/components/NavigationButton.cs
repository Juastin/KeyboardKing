using System.Windows;
using System.Windows.Controls;
using Controller;
using Model;

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
