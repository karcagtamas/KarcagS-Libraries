using KarcagS.Shared.Http;

namespace KarcagS.Common.Tools.HttpInterceptor;

public interface IErrorConverter
{
    HttpResultError ConvertException(Exception exception); 
}
