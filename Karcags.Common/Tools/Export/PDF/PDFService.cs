using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DinkToPdf.Contracts;

namespace Karcags.Common.Tools.Export.PDF
{
    public class PDFService : IPDFService
    {
        private readonly IConverter _converter;

        public PDFService(IConverter converter)
        {
            this._converter = converter;
        }
        
        /*public ExportResult GeneratePersonCsomor(List<CsomorPerson> persons)
        {
            persons = persons.OrderBy(x => x.Name).ToList();
            const string contentType = "application/pdf";
            string name = $"{DateHelper.ToFileName(DateTime.Now)}_persons.pdf";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Works"
            };

            var sb = new StringBuilder();

            sb.Append(@"<html><head></head><body>");
            sb.Append(@"<div class='header'>Person Table</div>");
            sb.Append(@"<table><tr><th class='empty'></th>");
            for (int i = 0; i < persons.Count; i++)
            {
                sb.AppendFormat(@"<th>{0}</th>", persons[i].Name);
            }

            sb.Append(@"</tr>");

            // Date Col
            foreach (var row in persons.SelectMany(x => x.Tables).GroupBy(x => x.Date).OrderBy(x => x.Key))
            {
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<th>{0}</th>", WriteHelper.HourInterval(row.Key, 1));

                foreach (var col in row)
                {
                    sb.AppendFormat(@"<td>{0}</td>", col.Work != null ? col.Work.Name : "-");
                }
                sb.Append(@"</tr>");
            }
            sb.Append(@"</table>");
            sb.Append(@"</body></html>");

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = sb.ToString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Generated Content" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] file = this._converter.Convert(pdf);

            return new ExportResult { Content = file, ContentType = contentType, FileName = name };
        }

        public ExportResult GenerateWorkCsomor(List<CsomorWork> works)
        {
            works = works.OrderBy(x => x.Name).ToList();
            const string contentType = "application/pdf";
            string name = $"{DateHelper.ToFileName(DateTime.Now)}_works.pdf";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Works"
            };

            var sb = new StringBuilder();

            sb.Append(@"<html><head></head><body>");
            sb.Append(@"<div class='header'>Work Table</div>");
            sb.Append(@"<table><tr><th class='empty'></th>");
            for (int i = 0; i < works.Count; i++)
            {
                sb.AppendFormat(@"<th>{0}</th>", works[i].Name);
            }

            sb.Append(@"</tr>");

            // Date Col
            foreach (var row in works.SelectMany(x => x.Tables).GroupBy(x => x.Date).OrderBy(x => x.Key))
            {
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<th>{0}</th>", WriteHelper.HourInterval(row.Key, 1));

                foreach (var col in row)
                {
                    sb.AppendFormat(@"<td>{0}</td>", col.Person != null ? col.Person.Name : "-");
                }
                sb.Append(@"</tr>");
            }
            sb.Append(@"</table>");
            sb.Append(@"</body></html>");

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = sb.ToString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Generated Content" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] file = this._converter.Convert(pdf);

            return new ExportResult { Content = file, ContentType = contentType, FileName = name };
        }*/
    }
}
