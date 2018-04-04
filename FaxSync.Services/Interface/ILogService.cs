using FaySync.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services.Interface
{
    public interface ILogService
    {
        void Log(ILogResults logResult);
        void Log(IEnumerable<ILogResults> logResult);
        int LogStartSession();
        void LogEndSession(int sessionId, bool result, string message);
    
    }
}
