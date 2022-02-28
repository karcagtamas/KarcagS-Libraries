namespace KarcagS.Common.Tools.Export.Excel;

public interface IExcelService
{
    ExportResult? GenerateTableExport<T>(IEnumerable<T> objectList, IEnumerable<Header> columnList,
        string fileName, bool appendCurrentDate);
}
