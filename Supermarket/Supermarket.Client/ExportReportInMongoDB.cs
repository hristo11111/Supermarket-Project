using MongoDB.Bson;
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
                var reports = from sale in dbSql.Sales.Include("Products").Include("Vendors")
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

                foreach (var report in reports)
                {
                    string jsonReports = report.ToJson();
                    jsonReports = jsonReports.Replace('_', '-');
                    jsonReports = jsonReports.Replace(",", ",\n");
                    AddInMongoDB(jsonReports);
                    File.WriteAllText(pathString + "/" + report.product_id + ".json",
                        jsonReports);
                }
            }
        }

        private static void AddInMongoDB(string report)
        {
            var mongoClient = new MongoClient("mongodb://localhost/");
            var mongoServer = mongoClient.GetServer();
            var productReports = mongoServer.GetDatabase("Product-Reports");

            var reports = productReports.GetCollection("reports");

            BsonDocument document = BsonDocument.Parse(report);
            reports.Insert<BsonDocument>(document);

            ListMongoDB(reports);
        }

        private static void ListMongoDB(MongoCollection reports)
        {
            var query = (from report in reports.AsQueryable<TotalReport>()
                         select report);

            foreach (var report in query)
            {
                Console.WriteLine(report);
            }
        }

        public static void Write(IEnumerable<TotalReport> data)
        {
            string conString = @"Data Source=supermarket.db;Version=3;";
            var dbSqLiteConnection = new SQLiteConnection(conString);
            try
            {
                dbSqLiteConnection.Open();

                foreach (var item in data)
                {
                  //  string commandText = String.Format("INSERT INTO Taxes VALUES(\"{0}\",{1});", item.ProductName, item.Tax);

                  //  SQLiteCommand cmd = new SQLiteCommand(commandText, dbSqLiteConnection);
                  //  var result = cmd.ExecuteNonQuery();
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
