using System;
using KeyboardKing.data_context;
using KeyboardKing.data_context.event_args;

namespace Controller
{
    public static class NavigationController
    {
        public static event NavigateHandler Navigate;
        public static event EventHandler ThemeChange;
        public static Pages CurrentPage { get; set; } = Pages.Empty;

        public static void NavigateToPage(Pages newPage)
        {
            Navigate?.Invoke(new NavigateEventArgs(CurrentPage, newPage));
            CurrentPage = newPage;
        }

        public static void ChangeTheme()
        {
            ThemeChange?.Invoke(null, new EventArgs());
        }
    }
}
