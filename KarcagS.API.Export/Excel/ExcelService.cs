using ClosedXML.Excel;
using KarcagS.API.Export.Models;

namespace KarcagS.API.Export.Excel;

/// <summary>
/// Excel Service
/// </summary>
public class ExcelService : IExcelService
{
    /// <summary>
    /// Generate table export
    /// </summary>
    /// <param name="objectList">Object table</param>
    /// <param name="columnList">Export columns</param>
    /// <param name="fileName">Destination file name</param>
    /// <param name="appendCurrentDate">Add current date to the name</param>
    /// <typeparam name="T">Type of object list</typeparam>
    /// <returns>File stream</returns>
    public ExportResult? GenerateTableExport<T>(IEnumerable<T> objectList, IEnumerable<Header> columnList, string fileName, bool appendCurrentDate)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(fileName);

        // Header
        var columns = columnList.ToList();
        for (var i = 0; i < columns.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = columns[i].DisplayName;
        }

        var objects = objectList.ToList();
        for (var i = 0; i < objects.Count; i++)
        {
            for (var j = 0; j < columns.Count; j++)
            {
                worksheet.Cell(2 + i, j + 1).Value = columns[j].GetValue(objects[i]);
            }
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return null;
    }
}