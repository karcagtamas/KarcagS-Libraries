using System;
using System.Collections.Generic;

namespace Karcags.Common.Tools.Services
{
    public interface ILoggerService
    {
        void LogError(Exception e);
        void LogInformation(string user, string service, string action, int id);
        void LogInformation(string user, string service, string action, int id, object entity);
        void LogInformation(string user, string service, string action, string id);
        void LogInformation(string user, string service, string action, string id, object entity);
        void LogInformation(string user, string service, string action, List<string> ids);
        void LogInformation(string user, string service, string action, List<string> ids, object entity);
        void LogInformation(string user, string service, string action, List<int> ids);
        void LogInformation(string user, string service, string action, List<int> ids, object entity);
        void LogAnonymousInformation(string service, string action, int id);
        void LogAnonymousInformation(string service, string action, int id, object entity);
        void LogAnonymousInformation(string service, string action, string id);
        void LogAnonymousInformation(string service, string action, string id, object entity);
        void LogAnonymousInformation(string service, string action, List<string> ids);
        void LogAnonymousInformation(string service, string action, List<string> ids, object entity);
        void LogAnonymousInformation(string service, string action, List<int> ids);
        void LogAnonymousInformation(string service, string action, List<int> ids, object entity);
        MessageException LogInvalidThings(string user, string service, string thing, string message);
        MessageException LogAnonymousInvalidThings(string service, string thing, string message);
        string AddUserToMessage(string message, string user);
        ErrorResponse ExceptionToResponse(Exception e, params Exception[] list);
    }
}