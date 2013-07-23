using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Supermarket.Client
{
    public static class SQLiteClient
    {
        public static void Read(string qry)
        {
            string conString = @"Data Source=c:\sqlite\supermarket.db;Version=3;";
            var dbSqLiteConnection = new SQLiteConnection(conString);
            try
            {
                dbSqLiteConnection.Open();
                string commandText = qry;
                SQLiteCommand cmd = new SQLiteCommand(commandText, dbSqLiteConnection);
                var result = cmd.ExecuteReader();

                while (result.Read())
                {
                    Console.WriteLine(result["ProductName"].ToString() + " - " + result["Tax"].ToString());
                }
            }
            catch (SQLiteException ex)
            {
            }
            finally
            {
                dbSqLiteConnection.Close();
            }
        }

        public static void Write(IEnumerable<TaxesData> data)
        {
            string conString = @"Data Source=c:\sqlite\supermarket.db;Version=3;";
            var dbSqLiteConnection = new SQLiteConnection(conString);
            try
            {
                dbSqLiteConnection.Open();

                foreach (var item in data)
                {
                    string commandText = String.Format("INSERT INTO Taxes VALUES(\"{0}\",{1});", item.ProductName, item.Tax);

                    SQLiteCommand cmd = new SQLiteCommand(commandText, dbSqLiteConnection);
                    var result = cmd.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
            }
            finally
            {
                dbSqLiteConnection.Close();
            }
        }
    }
}