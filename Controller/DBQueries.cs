using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class DBQueries
    {
        /// <summary>
        /// Example query.
        /// </summary>
        public static List<List<string>> GetAllUsers()
        {
            return DBHandler.Query("SELECT id, username FROM [dbo].[User]");
        }

        public static List<List<string>> GetAllEpisodes()
        {
            return DBHandler.Query("SELECT [dbo].[Chapter].name, episode, [dbo].[Episode].name FROM [dbo].[Episode]" +
                                   "LEFT JOIN [dbo].[Chapter]" +
                                   "ON [dbo].[Episode].chapterid = [dbo].[Chapter].id");
        }
    }
}
