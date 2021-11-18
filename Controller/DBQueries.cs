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
    }
}
