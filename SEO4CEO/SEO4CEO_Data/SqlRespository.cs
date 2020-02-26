using log4net;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace SEO4CEO_Data
{
    public class SqlRespository
    {
        private readonly static ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string dbSource = "./SqliteDB.db";
        public void firstRun()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();

            //Use DB in project directory.  If it does not exist, create it:
            connectionStringBuilder.DataSource = dbSource;

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                //Create a table (drop if already exists first):

                var delTableCmd = connection.CreateCommand();
                delTableCmd.CommandText = "DROP TABLE IF EXISTS Seo_Results";
                delTableCmd.ExecuteNonQuery();

                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = @"CREATE TABLE Seo_Results(Id INTEGER PRIMARY KEY,
Hits INTEGER,
TopPosition INTEGER,
DateTime INTEGER)";
                createTableCmd.ExecuteNonQuery();

                //Seed some data:
                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();

                    insertCmd.CommandText = "INSERT INTO Seo_Results VALUES('')";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT INTO favorite_beers VALUES('JAI ALAI IPA')";
                    insertCmd.ExecuteNonQuery();

                    insertCmd.CommandText = "INSERT INTO favorite_beers VALUES('RANGER IPA')";
                    insertCmd.ExecuteNonQuery();

                    transaction.Commit();
                }

                //Read the newly inserted data:
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT name FROM favorite_beers";

                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var message = reader.GetString(0);
                        Console.WriteLine(message);
                    }
                }


            }
        }
        public bool InsertSearchResults(int hits,int topPosition)
        {
            var success = true;
            try
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder();
                using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();

                    //Seed some data:
                    using (var transaction = connection.BeginTransaction())
                    {
                        var insertCmd = connection.CreateCommand();

                        insertCmd.CommandText = $"INSERT INTO Seo_Results (Hits,TopPosition,DateTime) " +
                        $"VALUES({hits}, {topPosition}, {DateTime.UtcNow})";

                        insertCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
            }
            catch(SqliteException sqlex)
            {
                _log.Warn("DB error when querying or connecting to db", sqlex);
                success = false;
            }
            catch(Exception ex)
            {
                _log.Warn(ex.Message);
                success = false;
            }
            return success;
        }

        public IEnumerable<SeoResult> GetTopResults()
        {
            var seoResults = new List<SeoResult>();
            try
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder();
                connectionStringBuilder.DataSource = dbSource;

                //Use DB in project directory.  If it does not exist, create it:
                connectionStringBuilder.DataSource = dbSource;

                using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                {
                    var selectCmd = connection.CreateCommand();
                    selectCmd.CommandText = "SELECT Id," +
                        "Hits," +
                        "TopPosition," +
                        "DateTime" +
                        "FROM Seo_Results" +
                        "ORDER BY TopPosition desc" +
                        "LIMIT 3";

                    using (var reader = selectCmd.ExecuteReader())
                    {
                        var datatable = new DataTable();
                        datatable.Load(reader);
                        var result = new SeoResult();
                        while (reader.Read())
                        {
                            result.Hits = reader.GetInt32("Hits");
                            result.TopPosition = reader.GetInt32("TopPosition");
                            result.DateTimeUtc = reader.GetInt32("DateTime");
                            seoResults.Add(result);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message, ex);
                
            }
            return seoResults;
        }
    }
}
