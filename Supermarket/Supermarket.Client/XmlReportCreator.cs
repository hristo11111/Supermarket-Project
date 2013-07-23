using Supermarket_EF.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace Supermarket.Client
{
    public static class XmlReportCreator
    {
        public static void CreateReport()
        {
            string path = "../../../Report.xml";
            Encoding encoding = Encoding.GetEncoding("windows-1251");
            CultureInfo nonInvariantCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = nonInvariantCulture;

            using (var data = new SupermarketEntities())
            {
                using (XmlTextWriter writer = new XmlTextWriter(path, encoding))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.IndentChar = '\t';
                    writer.Indentation = 2;
                    writer.WriteStartDocument();
                    writer.WriteStartElement("sales");

                    var salesByVendor = data.Sales.GroupBy(y => y.Location).ToList();
                    foreach (var sales in salesByVendor)
                    {
                        writer.WriteStartElement("sale");
                        writer.WriteAttributeString("vendor", sales.First().Location.LocationName);
                        foreach (var sale in sales)
                        {
                            writer.WriteStartElement("summary");

                            writer.WriteAttributeString("date", sale.Date.Value.ToString("dd-MMM-yyyy"));
                            writer.WriteAttributeString("total-sum", sale.Sum.ToString("{0:0.00}"));
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }
            }
        }
    }
}