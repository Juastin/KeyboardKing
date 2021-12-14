using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using Cryptography;

namespace Controller
{
    /// !!! NEVER USE THIS CLASS DIRECTLY, ONLY THROUGH DBQueries !!!
    internal static class DBHandler
    {
        /// <summary>
        /// Database connection.
        /// </summary>
        private static string _connection { get; set; } = ConfigurationManager.AppSettings["connectionString"];

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
            SqlConnection connection = null;
            SqlDataReader rdr = null;
            List<List<string>> result = new List<List<string>>();

            try
            {
                connection = OpenConnection(_connection);
                cmd.Connection = connection;
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
                rdr?.Close();
                connection?.Close();
            }
            return result;
        }

        // Query for Insert, Update and Delete
        public static bool Query(SqlCommand cmd)
        {
            SqlConnection connection = null;
            try
            {
                connection = OpenConnection(_connection);
                cmd.Connection = connection;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection?.Close();
            }
        }
        // Query for scalar queries
        public static T QueryScalar<T>(SqlCommand cmd)
        {
            SqlConnection connection = null;
            try
            {
                connection = OpenConnection(_connection);
                cmd.Connection = connection;
                cmd.Prepare();
                return (T)cmd.ExecuteScalar();
            }
            catch
            {
                return default;
            }
            finally
            {
                connection?.Close();
            }
        }

        public static SqlConnection OpenConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection(TripleDES.Decrypt(connectionString, "332cc6da-d757-4e80-a726-0bf6b615df09"));
            connection.Open();
            return connection;
        }
    }
}