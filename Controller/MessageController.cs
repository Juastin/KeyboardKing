using Model;

namespace Controller
{
    public static class MessageController
    {
        public static void Show(Pages info_page, string message, Pages redirect_page, int? time)
        {
            Session.Add("MessagePageInfo", new object[]{message, redirect_page, time});
            NavigationController.NavigateToPage(info_page);
        }
    }
}
