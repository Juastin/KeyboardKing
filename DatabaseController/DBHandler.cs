using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using Cryptography;
using System;

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
        public static List<List<string>> SelectQuery(MySqlCommand cmd)
        {
            MySqlConnection connection = null;
            MySqlDataReader rdr = null;
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
        public static bool Query(MySqlCommand cmd)
        {
            MySqlConnection connection = null;
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
        public static T QueryScalar<T>(MySqlCommand cmd)
        {
            MySqlConnection connection = null;
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

        // Query for scalar queries
        public static int InsertAndGet(MySqlCommand cmd)
        {
            MySqlConnection connection = null;
            try
            {
                connection = OpenConnection(_connection);
                cmd.Connection = connection;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
            catch (Exception e)
            {
                return default;
            }
            finally
            {
                connection?.Close();
            }
        }

        public static MySqlConnection OpenConnection(string connectionString)
        {
            MySqlConnection connection = new MySqlConnection(TripleDES.Decrypt(connectionString, "730cec9c-b95d-4647-b4a9-e7642b15c239"));
            connection.Open();
            return connection;
        }
    }
}