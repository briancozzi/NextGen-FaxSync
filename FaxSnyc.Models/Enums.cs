using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Models
{
    public enum ActionSyncType
    {
        AddUser,
        RemoveUser
    }
    public enum ActionSyncReason
    {
        FaxNumberChange,
        AssistantChange,
        FaxAndAssistantChange,
    }

    public enum LogType
    {
        ApiCall,
        Audit
    }
}
