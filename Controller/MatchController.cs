using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class MatchController
    {
        public static bool CheckIfUserExists()
        {
            string[] user = (string[])Session.Get("student");
            return DBQueries.GetAllUsersInMatch().Select(match => match.Contains(user[0])).FirstOrDefault();
        }
        public static void MakeMatch(int selectedValue)
        {
            DBQueries.AddMatchProgress(DBQueries.AddMatch(selectedValue, (string[])Session.Get("student")), (string[])Session.Get("student"));
        }
    }
}
