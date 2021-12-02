using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.event_args;

namespace Controller
{
    public static class NavigateController
    {
        public static event NavigateHandler Navigate;
        public static Pages CurrentPage { get; set; } = Pages.Empty;

        public static void NavigateToPage(Pages newPage)
        {
            Navigate?.Invoke(new NavigateEventArgs(CurrentPage, newPage));
            CurrentPage = newPage;
        }
    }
}
