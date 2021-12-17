using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Karcags.Common.Tools.Services
{
    /// <summary>
    /// Logger Service
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;
        private readonly IUtilsService _utilsService;

        /// <summary>
        /// Injector Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="utilsService">Utils Service</param>
        public LoggerService(ILogger<LoggerService> logger, IUtilsService utilsService)
        {
            this._logger = logger;
            this._utilsService = utilsService;
        }

        /// <summary>
        /// Join user to the given message
        /// </summary>
        /// <param name="message">Source message</param>
        /// <param name="user">User for join</param>
        /// <returns>Joined message</returns>
        public string AddUserToMessage(string message, string user)
        {
            return $"Invalid action for user {user}: {message}";
        }

        /// <summary>
        /// Log error to the console
        /// </summary>
        /// <param name="e">Exception for logging</param>
        /// <returns>Error Response from Exception</returns>
        public void LogError(Exception e)
        {
            this._logger.LogError(e.Message);
        }

        /// <summary>
        /// Log executed events
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="id">Entity Id</param>
        /// <param name="entity">Connected entity object</param>
        public void LogInformation(string user, string service, string action, int id, object entity)
        {
            this.LogInformation(user, service, action, id.ToString(), entity);
        }

        /// <summary>
        /// Log executed events
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="id">Entity Id</param>
        public void LogInformation(string user, string service, string action, int id)
        {
            this.LogInformation(user, service, action, id, null);
        }

        /// <summary>
        /// Exception to Error response.
        /// To API sending.
        /// </summary>
        /// <param name="e">Error</param>
        /// <param name="list">Other exception list</param>
        /// <returns>API Error response</returns>
        public ErrorResponse ExceptionToResponse(Exception e, params Exception[] list)
        {
            foreach (var error in list)
            {
                this.LogError(error);
            }

            this.LogError(e);
            return new ErrorResponse(e);
        }

        /// <summary>
        /// Log invalid things.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="thing">Thing</param>
        /// <param name="message">Log message</param>
        /// <returns>Invalid message</returns>
        public MessageException LogInvalidThings(string user, string service, string thing, string message)
        {
            string end = $"Invalid {thing}";
            this._logger.LogError($"{user}: {service} - {end}");
            return new MessageException(message);
        }

        /// <summary>
        /// Log invalid things.
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="thing">Thing</param>
        /// <param name="message">Log message</param>
        /// <returns>Invalid message</returns>
        public MessageException LogAnonymousInvalidThings(string service, string thing, string message)
        {
            string end = $"Invalid {thing}";
            this._logger.LogError($"Anonymous: {service} - {end}");
            return new MessageException(message);
        }


        /// <summary>
        /// Log executed events
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="id">Id of element</param>
        public void LogInformation(string user, string service, string action, string id)
        {
            this.LogInformation(user, service, action, id, null);
        }

        /// <summary>
        /// Log executed events
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="id">Id of element</param>
        /// <param name="entity">Connected entity object</param>
        public void LogInformation(string user, string service, string action, string id, object entity)
        {
            this._logger.LogInformation(
                $"{user}: {service} - {action.ToUpper()} - with id: {id}");
            if (entity != null)
            {
                this._logger.LogInformation(entity.ToString());
            }
        }

        /// <summary>
        /// Log executed events
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="ids">Ids of elements</param>
        /// <param name="entity">Connected entity object</param>
        public void LogInformation(string user, string service, string action, List<string> ids, object entity)
        {
            this.LogInformation(user, service, action, string.Join(", ", ids), entity);
        }

        /// <summary>
        /// Log executed events
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="ids">Ids of elements</param>
        public void LogInformation(string user, string service, string action, List<string> ids)
        {
            this.LogInformation(user, service, action, ids, null);
        }

        /// <summary>
        /// Log executed events
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="ids">Ids of elements</param>
        /// <param name="entity">Connected entity object</param>
        public void LogInformation(string user, string service, string action, List<int> ids, object entity)
        {
            this.LogInformation(user, service, action, ids.Select(x => x.ToString()).ToList(), entity);
        }

        /// <summary>
        /// Log executed events
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="ids">Ids of elements</param>
        public void LogInformation(string user, string service, string action, List<int> ids)
        {
            this.LogInformation(user, service, action, ids, null);
        }

        /// <summary>
        /// Log executed events as anonymous
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="id">Ids of elements</param>
        public void LogAnonymousInformation(string service, string action, int id)
        {
            this.LogAnonymousInformation(service, action, id, null);
        }

        /// <summary>
        /// Log executed events as anonymous
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="id">Id of element</param>
        /// <param name="entity">Entity</param>
        public void LogAnonymousInformation(string service, string action, int id, object entity)
        {
            this.LogAnonymousInformation(service, action, id.ToString(), entity);
        }

        /// <summary>
        /// Log executed events as anonymous
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="id">Id of element</param>
        public void LogAnonymousInformation(string service, string action, string id)
        {
            this.LogAnonymousInformation(service, action, id, null);
        }

        /// <summary>
        /// Log executed events as anonymous
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="id">Id of element</param>
        /// <param name="entity">Entity</param>
        public void LogAnonymousInformation(string service, string action, string id, object entity)
        {
            this._logger.LogInformation($"Anonymous: {service} - {action.ToUpper()} - with id: {id}");
            if (entity != null)
            {
                this._logger.LogInformation(entity.ToString());
            }
        }

        /// <summary>
        /// Log executed events as anonymous
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="ids">Id list of elements</param>
        public void LogAnonymousInformation(string service, string action, List<string> ids)
        {
            this.LogAnonymousInformation(service, action, ids, null);
        }

        /// <summary>
        /// Log executed events as anonymous
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="ids">Id list of elements</param>
        /// <param name="entity">Entity</param>
        public void LogAnonymousInformation(string service, string action, List<string> ids, object entity)
        {
            this.LogAnonymousInformation(service, action, string.Join(", ", ids), entity);
        }

        /// <summary>
        /// Log executed events as anonymous
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="ids">Id list of elements</param>
        public void LogAnonymousInformation(string service, string action, List<int> ids)
        {
            this.LogAnonymousInformation(service, action, ids, null);
        }

        /// <summary>
        /// Log executed events as anonymous
        /// </summary>
        /// <param name="service">Service's name</param>
        /// <param name="action">Executed action</param>
        /// <param name="ids">Id list of elements</param>
        /// <param name="entity">Entity</param>
        public void LogAnonymousInformation(string service, string action, List<int> ids, object entity)
        {
            this.LogAnonymousInformation(service, action, ids.Select(x => x.ToString()).ToList(), entity);
        }
    }
}