using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Supermarket_EF.Data;
using System;
using System.IO;
using System.Linq;


namespace Supermarket.Client
{
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
                                   select new
                                     {
                                         productId = g.Key.ProductID,
                                         productName = g.Key.ProductName,
                                         vendorName = g.Key.VendorName,
                                         totalSoldQuantitySold = totalSold,
                                         totalIncomes = totalIncome
                                     };

                string pathString = "../../../Product-Reports";

                System.IO.Directory.CreateDirectory(pathString);

                foreach (var report in reports)
                {
                    string jsonReports = report.ToJson();
                    AddInMongoDB(jsonReports);
                    File.WriteAllText(pathString + "/"  + report.productId + ".json",
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

           // ListMongoDB(reports);
        }

        //private static void ListMongoDB(MongoCollection reports)
        //{
        //    var query = (from report in reports.AsQueryable<BsonDocument>()
        //                 select report);

        //    foreach (var report in query)
        //    {
        //        Console.WriteLine(report);                
        //    }
        //}
    }
}
