using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket_EF.Data;
using System.Xml;
using System.Globalization;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Data.SQLite;

namespace Supermarket.Client
{
    public class VendorExpenses
    {
        public static void LoadVendorExpenses()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Vendors-Expenses.xml");

            XmlNode rootNode = doc.DocumentElement;

            List<Expens> expenses = new List<Expens>();
            using (var db = new SupermarketEntities())
            {
                string vendorName;
                var mongoClient = new MongoClient("mongodb://localhost/");
                var mongoServer = mongoClient.GetServer();
                var productReports = mongoServer.GetDatabase("Product-Reports");
                var reports = productReports.GetCollection("expenses");

                foreach (XmlNode item in rootNode.ChildNodes)
                {
                    XmlAttribute atr = item.Attributes[0];
                    vendorName = atr.Value;

                    int id = db.Vendors.Where(x => x.VendorName == vendorName).Select(x => x.ID).FirstOrDefault();

                    foreach (XmlNode node in item.ChildNodes)
                    {
                        Expens expense = new Expens();
                        string atrDate = node.Attributes[0].Value.ToString();
                        string dateFormat = "MMM-yyyy";
                        DateTime date = DateTime.ParseExact(atrDate, dateFormat, CultureInfo.InvariantCulture);

                        decimal expenseValue = decimal.Parse(node.InnerText);

                        expense.Date = date;
                        expense.Expenses = expenseValue;
                        expense.VendorId = id;



                        expenses.Add(expense);

                        ExpenseByName expenseByName = new ExpenseByName();
                        expenseByName.VendorName = vendorName;
                        expenseByName.Date = expense.Date;
                        expenseByName.Expenses = expense.Expenses;

                        reports.Insert(expenseByName);  
                    }
                }



                foreach (Expens item in expenses)
                {
                    db.Expenses.Add(item);
                }

                db.SaveChanges();

                WriteToSqlLite(reports);
            }
        }

        private static void WriteToSqlLite(MongoCollection totalExpenses)
        {
            string conString = @"Data Source=../../../supermarket.db;Version=3;";
            var dbSqLiteConnection = new SQLiteConnection(conString);
            try
            {
                dbSqLiteConnection.Open();
                var query = totalExpenses.FindAllAs<ExpenseByName>()
                    .Where(x => x.Date.ToString("MM-yyyy") == "06-2013")
                    .Select(y => new { Expenses = y.Expenses, VendorName = y.VendorName });

                foreach (var item in query)
                {
                    string commandText = String.Format(@"INSERT INTO TotalExpenses VALUES(""{0}"",{1});", item.VendorName, item.Expenses);
                    SQLiteCommand cmd = new SQLiteCommand(commandText, dbSqLiteConnection);
                    var result = cmd.ExecuteNonQuery();
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


        private class ExpenseByName
        {
            [BsonId]
            public ObjectId Id { get; set; }

            public string VendorName { get; set; }
            public DateTime Date { get; set; }
            public decimal Expenses { get; set; }

        }
    }
}
