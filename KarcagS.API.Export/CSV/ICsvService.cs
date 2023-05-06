using KarcagS.API.Export.Models;

namespace KarcagS.API.Export.CSV;

public interface ICsvService
{
    ExportResult? GenerateTableExport<T>(IEnumerable<T> objectList, IEnumerable<Header> columnList,
        string fileName, bool appendCurrentDate);
}
