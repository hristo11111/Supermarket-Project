using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupermarketOpenAccess.Models;
using System.Data.Entity;
using Supermarket_EF.Data;
using Ionic.Zip;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace Supermarket.Client
{

    class MainApp
    {
        static void Main()
        {
            using (var db = new EntitiesModel())
            {
                var vendors = db.Vendors;
                var products = db.Products;
                var measures = db.Measures;

                using (var dbSql = new SupermarketEntities())
                {
                    //foreach (var vendor in vendors)
                    //{
                    //    Supermarket_EF.Data.Vendor vendorObj = new Supermarket_EF.Data.Vendor();
                    //    vendorObj.ID = vendor.IdvendorsID;
                    //    vendorObj.VendorName = vendor.VendorName;
                    //    dbSql.Vendors.Add(vendorObj);
                    //}

                    //foreach (var measure in measures)
                    //{
                    //    Supermarket_EF.Data.Measure measureObj = new Supermarket_EF.Data.Measure()
                    //    {
                    //        ID = measure.ID,
                    //        MeasureName = measure.MeasureName
                    //    };
                    //    dbSql.Measures.Add(measureObj);
                    //}

                    //foreach (var product in products)
                    //{
                    //    Supermarket_EF.Data.Product productObj = new Supermarket_EF.Data.Product()
                    //    {
                    //        ID = product.ID,
                    //        VendorID = product.VendorID,
                    //        ProductName = product.ProductName,
                    //        MeasureID = product.MeasureID,
                    //        BasePrice = (decimal)product.BasePrice
                    //    };
                    //    dbSql.Products.Add(productObj);
                    //}

                    //dbSql.SaveChanges();

                    //MyExtract();

                    string dirPath = "../../../Extracted Files";
                    var dir = Directory.GetDirectories(dirPath);

                    Traversed(dir);
                }
            }
        }

        private static void Traversed(string[] dir)
        {
            foreach (var subdir in dir)
            {
                var files = Directory.GetFiles(subdir);

                foreach (var file in files)
                {
                    ReadWriteExcell(file);
                }
            }
        }

        private static void ReadWriteExcell(string file)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file +
                @";Extended Properties=""Excel 12.0 Xml;HDR=Yes;""";

            OleDbConnection dbCon = new OleDbConnection(connectionString);

            dbCon.Open();

            using (dbCon)
            {
                
                OleDbCommand readTable = new OleDbCommand("SELECT * FROM [Sales$]", dbCon);
                OleDbDataReader reader = readTable.ExecuteReader();

                using (var db = new SupermarketEntities())
                {
                    using (reader)
                    {
                        reader.Read();
                        string location = reader[0].ToString();

                        while (reader.Read())
                        {

                        }
                    }
                }


                Console.WriteLine();

                
            }
        }

        private static void MyExtract()
        {
            string zipToUnpack = "../../../Sample-Sales-Reports.zip";
            string unpackDirectory = "../../../Extracted Files";
            using (ZipFile zipFile = ZipFile.Read(zipToUnpack))
            {
                // here, we extract every entry, but we could extract conditionally
                // based on entry name, size, date, checkbox status, etc.  
                foreach (ZipEntry e in zipFile)
                {
                    e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

    }
}

