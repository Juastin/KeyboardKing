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
        public static User Student { get; set; }
        public static event EventHandler Refresh;
        public static void Initialise()
        {
            Student = (User)Session.Get("student");
            Refresh?.Invoke(null,EventArgs.Empty);
        }
        public static void ChangeDyslectic(bool dyslectic)
        {
            DBQueries.UpdateDyslecticSettings(Student.Id, dyslectic);
        }
    }
}
