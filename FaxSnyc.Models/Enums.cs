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
        UpdateUser,
        RemoveUser,
        AssignUser,
        DeAssignUser
    }
    public enum ActionSyncReason
    {
        NewUser,
        UserIsDisabled,
        UserIsRemovedFromAd,
        UserIsUpdated,
        UpdateUserFaxGroup,
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
