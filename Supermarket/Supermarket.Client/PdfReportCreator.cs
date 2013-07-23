using System;
using System.Linq;
using System.Text;
using System.IO;
using Supermarket_EF.Data;
using Supermarket.Client.Helpers;
using iTextSharp.text;

namespace Supermarket.Client
{
    public static class PdfReportCreator
    {
        public static void CreatePDFs()
        {
            Console.WriteLine("PDF Report creation started...");
            StringBuilder sb = new StringBuilder();
            using (var dbEF = new SupermarketEntities())
            {
                sb.Append("<table cellpadding='5' border='1'>");
                sb.Append("<tr><th align='center'><b>Aggregated Sales Report</b></th></tr>");
                sb.Append("</table>");
                sb.Append("<table cellpadding='5' border='1'>");

                decimal grandTotal = 0M;

                var db = dbEF.Sales.Select(x => x.Date).OrderBy(d => d.Value).Distinct();
                foreach (var dateTime in db)
                {
                    var date = DateTime.Parse(dateTime.ToString()).ToShortDateString();
                    //Console.WriteLine(dateTime);
                    sb.AppendFormat("<tr bgcolor='silver'><td colspan='5'>Date: {0}</td></tr>", date);
                    sb.Append("<tr bgcolor='silver'>");
                    sb.Append("<td><b>Product</b></td>");
                    sb.Append("<td><b>Quantity</b></td>");
                    sb.Append("<td><b>Unit Price</b></td>");
                    sb.Append("<td><b>Location</b></td>");
                    sb.Append("<td><b>Sum</b></td>");
                    sb.Append("</tr>");

                    var d = dbEF.Sales.Where(x => x.Date == dateTime);
                    decimal sum = 0M;
                    foreach (var item in d)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", item.Product.ProductName);
                        sb.AppendFormat("<td>{0}</td>", item.Quanity);
                        sb.AppendFormat("<td>{0:F2}</td>", item.UnitPrice);
                        sb.AppendFormat("<td>{0}</td>", item.Location.LocationName);
                        sb.AppendFormat("<td>{0:F2}</td>", item.Sum);
                        sb.Append("</tr>");
                        sum += item.Sum;
                    }
                    grandTotal += sum;
                    sb.Append("<tr><td>...</td><td>...</td><td>...</td><td>...</td><td>...</td></tr>");
                    sb.AppendFormat("<tr><td colspan='4' align='right'>Total sum for {0}:</td><td><b>{1:F2}</b></td></tr>", date, sum);
                }
                sb.AppendFormat("<tr><td colspan='4' align='right'>Grand Total:</td><td><b>{0:F2}</b></td></tr>", grandTotal);

                sb.Append("</table>");
                Console.WriteLine("PDF Report generated.");
            }

            PDFBuilder.HtmlToPdfBuilder builder = new PDFBuilder.HtmlToPdfBuilder(PageSize.LETTER);
            PDFBuilder.HtmlPdfPage page = builder.AddPage();
            page.AppendHtml(sb.ToString());
            byte[] file = builder.RenderPdf();
            string tempFolder = "../../../PdfResult\\";
            string tempFileName = DateTime.Now.ToString("yyyy-MM-dd") + "-" + Guid.NewGuid() + ".pdf";
            if (Helpers.Helper.DirectoryExist(tempFolder))
            {
                if (!File.Exists(tempFolder + tempFileName))
                    File.WriteAllBytes(tempFolder + tempFileName, file);
            }
        }
    }
}
