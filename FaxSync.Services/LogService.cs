using FaxSync.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaySync.Models.Interface;

namespace FaxSync.Services
{
    public class LogService : ILogService, IEventLogerService
    {
        static LogService _logObject;
        static object lockObject;
        public static IEventLogerService GetLogService()
        {
            lock(lockObject)
            {       
               if(_logObject == null)
                {
                    _logObject = new LogService();
                }

                return _logObject;
            }
        }

        public void Log(IEnumerable<ILogResults> logResult)
        {
            foreach (var log in logResult)
            {
                Log(log);
            }
        }

        public void Log(ILogResults logResult)
        {
            LogEvent($"SID:{logResult.SessionId} Type:{logResult.Type.ToString()} User:{logResult.UserId} Result:{logResult.Result} {logResult.Message}");
        }

        public void LogEndSession(int sessionId, bool result, string message)
        {
           
        }

        public void LogEvent(string eventMessage)
        {
            try
            {
                Console.WriteLine($"{DateTime.Now} {eventMessage}");
            }
            finally
            { }
        }

        public int LogStartSession()
        {
            return 1;
        }
    }
}
