using System;
using Model;
using Model.event_args;

namespace Controller
{
    public static class NavigationController
    {
        public static event NavigateHandler Navigate;
        public static event NavigateHandler AfterNavigate;
        public static event EventHandler ThemeChange;

        public static Pages PreviousPage { get; set; } = Pages.Empty;
        public static Pages CurrentPage { get; set; } = Pages.Empty;

        public static void NavigateToPage(Pages newPage)
        {
            PreviousPage = CurrentPage;
            Navigate?.Invoke(new NavigateEventArgs(CurrentPage, newPage));
            CurrentPage = newPage;
            AfterNavigate?.Invoke(new NavigateEventArgs(CurrentPage, newPage));
        }

        public static void ChangeTheme()
        {
            ThemeChange?.Invoke(null, new EventArgs());
        }
    }
}
