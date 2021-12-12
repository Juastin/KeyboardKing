using Model;

namespace Controller
{
    public static class MessageController
    {
        public static void Show(Pages info_page, string message, Pages redirect_page, int? time)
        {
            Session.Add("MessagePageInfo", new UList(new object[]{message, redirect_page, time}));
            NavigationController.NavigateToPage(info_page);
        }
        public static void ShowConfirmation(Pages confirm_page, string message, Pages previous_page, Pages approved_page)
        {
            Session.Add("ConfirmationPageInfo", new UList(new object[] { message, previous_page, approved_page }));
            NavigationController.NavigateToPage(confirm_page);
        }
    }
}
