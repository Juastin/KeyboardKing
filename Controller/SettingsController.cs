using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseController;
using Model;

namespace Controller
{
    public static class SettingsController
    {
        public static event EventHandler Refresh;
        public static void Initialise()
        {
            Refresh?.Invoke(null,EventArgs.Empty);
        }

        public static bool DeleteAccount()
        {
            return DBQueries.DeleteUserAccount((User)Session.Get("student"));
        }
    }
}
