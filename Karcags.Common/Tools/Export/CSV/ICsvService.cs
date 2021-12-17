using System.Collections.Generic;

namespace Karcags.Common.Tools.Export.CSV
{
    public interface ICsvService
    {
        ExportResult GenerateTableExport<T>(IEnumerable<T> objectList, IEnumerable<Header> columnList,
            string fileName, bool appendCurrentDate);
    }
}