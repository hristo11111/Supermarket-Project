using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Supermarket_EF.Data;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;


namespace Supermarket.Client
{
    public class TotalReport
    {
        [BsonId]
        public ObjectId id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string vendor_name { get; set; }
        public int total_quantity_sold { get; set; }
        public decimal total_incomes { get; set; }
    }

    public static class ExportReportInMongoDB
    {
        public static void CreateReport()
        {
            using (var dbSql = new SupermarketEntities())
            {
                var reportsQuery = from sale in dbSql.Sales.Include("Products").Include("Vendors")
                                   orderby sale.ProductID
                                   group sale by new { sale.ProductID, sale.Product.ProductName, sale.Product.Vendor.VendorName }
                                       into g
                                       let totalSold = g.Sum(x => x.Quanity)
                                       let totalIncome = g.Sum(y => y.Sum)
                                       select new TotalReport
                                         {
                                             product_id = g.Key.ProductID,
                                             product_name = g.Key.ProductName,
                                             vendor_name = g.Key.VendorName,
                                             total_quantity_sold = totalSold,
                                             total_incomes = totalIncome
                                         };

                string pathString = "../../../Product-Reports";

                System.IO.Directory.CreateDirectory(pathString);

                var mongoClient = new MongoClient("mongodb://localhost/");
                var mongoServer = mongoClient.GetServer();
                var productReports = mongoServer.GetDatabase("Product-Reports");
                var reports = productReports.GetCollection("reports");
 
                foreach (var report in reportsQuery)
                {
                    reports.Insert<TotalReport>(report);

                    string jsonReports = report.ToJson();
                    jsonReports = jsonReports.Replace('_', '-');
                    jsonReports = jsonReports.Replace(",", ",\n");
                    File.WriteAllText(pathString + "/" + report.product_id + ".json",
                        jsonReports);
                }

                WriteToSqlLite(reports);
            }
        }       

        public static void WriteToSqlLite(MongoCollection reports)
        {
            string conString = @"Data Source=../../../supermarket.db;Version=3;";
            var dbSqLiteConnection = new SQLiteConnection(conString);
            try
            {
                dbSqLiteConnection.Open();
                var query = (from report in reports.AsQueryable<TotalReport>()
                             select report);

                foreach (var report in query)
                {
                    report.product_name = report.product_name.Replace("\"", "\"\"");
                    string commandText = String.Format(@"INSERT INTO TotalReports VALUES({0},""{1}"",""{2}"",{3}, {4});",
                        report.product_id, report.product_name, report.vendor_name, report.total_quantity_sold, report.total_incomes);

                    SQLiteCommand cmd = new SQLiteCommand(commandText, dbSqLiteConnection);
                    cmd.ExecuteNonQuery();
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
