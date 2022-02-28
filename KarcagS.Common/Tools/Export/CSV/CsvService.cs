using System.Text;

namespace KarcagS.Common.Tools.Export.CSV;

/// <summary>
/// CSV Service
/// </summary>
public class CsvService : ICsvService
{
    /// <summary>
    /// Generate table export
    /// </summary>
    /// <param name="objectList">Object list</param>
    /// <param name="columnList">Displayed columns</param>
    /// <param name="fileName">File name</param>
    /// <param name="appendCurrentDate">Append current date to file name</param>
    /// <typeparam name="T">Type of the table rows</typeparam>
    /// <returns>Generated file</returns>
    public ExportResult? GenerateTableExport<T>(IEnumerable<T> objectList, IEnumerable<Header> columnList,
        string fileName,
        bool appendCurrentDate)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(string.Join(",", columnList.Select(x => x.DisplayName).ToList()));


        foreach (var obj in objectList)
        {
            stringBuilder.AppendLine(string.Join(",", columnList.Select(x => x.GetValue(obj)).ToList()));
        }

        /*var result = new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString())),
            "text/csv")
        {
            FileDownloadName = appendCurrentDate
                ? $"{DateHelper.DateToString(DateTime.Now)}{fileName}.csv"
                : $"{fileName}.csv"
        };*/

        return null;
    }
}
