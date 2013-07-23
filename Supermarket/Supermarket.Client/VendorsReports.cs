using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Data.OleDb;

namespace Supermarket.Client
{
     public static class VendorsReports
    {
        public static void CreateExcel()
        {
            string conString = @"Data Source=../../../supermarket.db;Version=3;";
            var dbSqLiteConnection = new SQLiteConnection(conString);

            string conectionExcel = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\..\Products-Total-Report.xlsx;Extended Properties=""Excel 12.0 Xml;""";
            OleDbConnection con = new OleDbConnection(conectionExcel);
            con.Open();
            try
            {
                dbSqLiteConnection.Open();
                
                string query = @"SELECT  te.[VandorName], SUM(tr.[TotalIncomes]) as Incomes, te.[Expense]," +
                    " SUM(tr.[TotalIncomes] * t.[Tax]) as Taxes," +
                    " SUM(tr.[TotalIncomes]) - te.[Expense] - SUM(tr.[TotalIncomes] * t.[Tax]) as FinancialResult " +
                    "from  Taxes t Join TotalReports tr on t.[ProductName] = tr.[ProductName] " +
                    "Join TotalExpenses te on tr.[VendorName] = te.[VandorName] " +
                    "Group By te.[VandorName], te.[Expense]";

                SQLiteCommand cmd = new SQLiteCommand(query, dbSqLiteConnection);

                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    //string row = string.Format(@"INSERT INTO [Sheet1$] (Name, Score) VALUES (""{0}"", {1}, {2}, {3}, {4})",
                    //    result["VandorName"].ToString(), result["Incomes"].ToString(), result["Expense"].ToString(),
                    //    result["Taxes"].ToString(), result["FinancialResult"].ToString());
                    //OleDbCommand cmdExcel = new OleDbCommand(row, con);
                    //cmdExcel.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dbSqLiteConnection.Close();
            }
        }
    }

}
