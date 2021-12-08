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
        private static string _creatorId;
        private static int _amountOfPlayers;

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

        /// <summary>
        /// <para>This method will add a user in MatchProgress to the database</para>
        /// It does this by getting the id of the chosen Match and inserting it in MatchProgressr.
        /// </summary>
        public static void AddUserInMatchProgress(string selectedValue)
        {
            _currentMatchId = int.Parse(selectedValue);
            DBQueries.AddMatchProgress(_currentMatchId, (UList)Session.Get("student"));
        }

        /// <summary>
        /// <para>This method will remove a user in MatchProgress to the database</para>
        /// It does this by giving the Match and User id to the method to delete the MatchProgress.
        /// </summary>
        public static void RemoveUserInMatchProgress()
        {
            DBQueries.RemoveUserInMatch(_currentMatchId, (UList)Session.Get("student"));
        }

        /// <summary>
        /// <para>This method get current MatchProgressInfo</para>
        /// It does this by giving the Match id to get info from MatchProgress.
        /// </summary>
        public static List<List<string>> GetMatchProgressInfo()
        {
            List<List<string>> matchInfo = DBQueries.GetMatchProgress(_currentMatchId);
            _amountOfPlayers = matchInfo.Count();
            if (_amountOfPlayers != 0) _creatorId = matchInfo[0][8];
            return matchInfo;
        }

        public static bool CheckUserIsCreator()
        {
            UList student = (UList)Session.Get("student");
            return student.Get<string>(0).Equals(_creatorId, StringComparison.Ordinal);
        }

        public static void DeleteMatch()
        {
            DBQueries.RemoveUserInMatch(_currentMatchId, (UList)Session.Get("student"));
            DBQueries.DeleteMatch(_currentMatchId);
        }

        public static bool CheckCreatorIsAloneInMatch() { return _amountOfPlayers == 1; }

        public static int GetMatchId() { return _currentMatchId; }
    }
}