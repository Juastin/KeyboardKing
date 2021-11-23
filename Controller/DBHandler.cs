using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace Controller
{
    /// !!! NEVER USE THIS CLASS DIRECTLY, ONLY THROUGH DBQueries !!!
    internal static class DBHandler
    {
        /// <summary>
        /// Database connection.
        /// </summary>
        private static SqlConnection _connection {get;set;} = new SqlConnection(TripleDES.Decrypt(ConfigurationManager.AppSettings["connectionString"], "332cc6da-d757-4e80-a726-0bf6b615df09"));

        /// <summary>
        /// Used to query the DB.
        /// Example: "SELECT * FROM [dbo].[User]"
        /// 
        /// Returns a List<List<string>> like:
        /// {
        ///     {"1", "Username1"}
        ///     {"2", "Username2"}
        ///     {"3", "Username3"}
        /// }
        /// </summary>
        
        public static List<List<string>> SelectQuery(SqlCommand cmd)
        {
            SqlDataReader rdr = null;
            List<List<string>> result = new List<List<string>>();

            try
            {
                _connection.Open();
                cmd.Connection = _connection;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        temp.Add(rdr[i].ToString());
                    }
                    result.Add(temp);
                }
            }
            finally
            {
                if (rdr != null) { rdr.Close(); }
                if (_connection != null) { _connection.Close(); }
            }
            return result;
        }

        // Query for Insert, Update and Delete
        public static bool Query(SqlCommand cmd)
        {
            try
            {
                _connection.Open();
                cmd.Connection = _connection;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return true;
            } catch
            {
                return false;
            }
            finally
            {
                if (_connection != null) { _connection.Close(); }
            }
        }

    }
}