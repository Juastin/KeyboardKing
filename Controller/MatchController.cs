using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class MatchController
    {
        private static int _currentMatchId;

        /// <summary>
        /// <para>This method checks if there is already a user in a match.</para>
        /// It does this by running an sql query to get all user that are in a match and compare that to the user that is in the session (the logged in user)
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfUserExists()
        {
            UList student = (UList)Session.Get("student");
            return DBQueries.GetAllUsersInMatch().SelectMany(result => result.Where(data => data.Equals(student.Get<string>(0), StringComparison.Ordinal)).Select(data => new { })).Any();
        }
        /// <summary>
        /// <para>This method will make a match by adding a Match, and a MatchProgress to the database</para>
        /// It does this by getting the id of the just inserted Match and inserting it into MatchProgress with the current user.
        /// </summary>
        /// <param name="selectedValue"></param>
        public static void MakeMatch(int selectedValue)
        {
            UList student = (UList)Session.Get("student");
            _currentMatchId = DBQueries.AddMatch(selectedValue, student);
            DBQueries.AddMatchProgress(_currentMatchId, student);
        }

        public static int GetMatchId() { return _currentMatchId; }

        /// <summary>
        /// <para>This method will add a user in MatchProgress to the database</para>
        /// It does this by getting the id of the chosen Match and inserting it into MatchProgress with the logged user.
        /// </summary>
        public static void AddUserInMatchProgress(string selectedValue)
        {
            _currentMatchId = int.Parse(selectedValue);
            DBQueries.AddMatchProgress(_currentMatchId, (UList)Session.Get("student"));
        }

        public static void RemoveUserInMatchProgress()
        {
            DBQueries.RemoveUserInMatch(_currentMatchId, (UList)Session.Get("student"));
        }
    }
}