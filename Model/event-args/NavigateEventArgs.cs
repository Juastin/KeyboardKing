using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardKing.data_context.event_args
{
    public delegate void NavigateHandler(NavigateEventArgs args);

    public class NavigateEventArgs : EventArgs
    {
        public Pages OldPage { get; set; }
        public Pages NewPage { get; set; }
        public NavigateEventArgs(Pages oldPage, Pages newPage)
        {
            OldPage = oldPage;
            NewPage = newPage;
        }
    }
}
