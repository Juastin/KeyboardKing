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
using KeyboardKing.core;
using Model;

namespace KeyboardKing.areas.info
{
    /// <summary>
    /// Interaction logic for ConfirmationPage.xaml
    /// </summary>
    public partial class ConfirmationPage : JumpPage
    {
        private Pages _targetLocation;
        private Pages _previousLocation;

        public ConfirmationPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
            UList vars = (UList)Session.Get("ConfirmationPageInfo");

            TitleLabel.Text = vars.Get<string>(0);
            _targetLocation = vars.Get<Pages>(1);
            _previousLocation = vars.Get<Pages>(2);
        }

        public override void OnShadow()
        {
            Dispatcher.Invoke(() =>
            {
                TitleLabel.Text = "";
                _targetLocation = Pages.Empty;
                Session.Remove("ConfirmationPageInfo");
            });
        }

        public override void OnTick()
        {
        }

        private void RedirectApproved(object sender, RoutedEventArgs e)
        {
            NavigationController.NavigateToPage(_targetLocation);
        }

        private void RedirectDeclined(object sender, RoutedEventArgs e)
        {
            NavigationController.NavigateToPage(_previousLocation);
        }
    }
}
