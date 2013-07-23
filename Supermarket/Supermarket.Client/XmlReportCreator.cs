using Supermarket_EF.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace Supermarket.Client
{
    public static class XmlReportCreator
    {
        public static void CreateReport()
        {
            string path = "../../Report.xml";
            Encoding encoding = Encoding.GetEncoding("windows-1251");

            using (var dbSql = new SupermarketEntities())
            {
                using (XmlTextWriter writer = new XmlTextWriter(path, encoding))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.IndentChar = '\t';
                    writer.Indentation = 2;
                    writer.WriteStartDocument();
                    writer.WriteStartElement("sales");
                    var asd = dbSql.Sales.Where(s => s.ID == 1).FirstOrDefault();

                    var salesByVendor = dbSql.Sales.GroupBy(y => y.Location).ToList();
                    foreach (var sales in salesByVendor)
                    {
                        writer.WriteStartElement("sale");
                        writer.WriteAttributeString("vendor", sales.First().Location.ToString());

                        foreach (var sale in sales)
                        {
                            writer.WriteStartAttribute("summary");
                            writer.WriteAttributeString("date", sale.Date.ToString());
                            writer.WriteAttributeString("total-sum", sale.Sum.ToString());
                            writer.WriteEndAttribute();
                        }

                        writer.WriteEndAttribute();
                    }

                    writer.WriteEndAttribute();
                }
            }
        }
    }
}