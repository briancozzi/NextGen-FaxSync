using FaxSnyc.Models.Sync;
using FaxSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Domain
{
    public class ActionSync
    {
        public AssistantActionSync AssistantSnycObj { get; set; }
        public ActionSyncType ActionType { get; set; }
        public ActionSyncReason ActionReason { get; set; }
        public ApiResult Result  { get; set; }
        public ActionSync(ActionSyncType type, ActionSyncReason reason, string attorneyId, string userId, string faxNumber)
        {
            AssistantSnycObj = new AssistantActionSync();
            ActionType = type;
            ActionReason = reason;
            AssistantSnycObj.AttorneyId = attorneyId;
            AssistantSnycObj.UserId = userId;
            AssistantSnycObj.FaxNumber = faxNumber;
        }
        public void SetFaxSolutionsIds(int faxAttorneyUserId, int faxUserId, int faxNumberId)
        {
            AssistantSnycObj.FaxAttorneyUserId = faxAttorneyUserId;
            AssistantSnycObj.FaxNumberId = faxNumberId;
            AssistantSnycObj.FaxUserId = faxUserId;
        }
    }
}
