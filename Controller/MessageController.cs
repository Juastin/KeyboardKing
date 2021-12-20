using Model;

namespace Controller
{
    public static class MessageController
    {
        public static void Show(Pages infoPage, string message, Pages redirectPage, int time)
        {
            Session.Add("MessagePageInfo", new UList(new object[]{message, redirectPage, time}));
            NavigationController.NavigateToPage(infoPage);
        }
        public static void ShowPause(Pages pausePage,string message, Pages redirectPage)
        {
            Session.Add("PausePageInfo", new UList(new object[] {message, redirectPage}));
            NavigationController.NavigateToPage(pausePage);
        }
        public static void ShowConfirmation(Pages confirmPage, string message, Pages previousPage, Pages approvedPage)
        {
            Session.Add("ConfirmationPageInfo", new UList(new object[] { message, previousPage, approvedPage }));
            NavigationController.NavigateToPage(confirmPage);
        }
    }
}
