using FaxSnyc.Models.Sync;
using FaxSync.Models;
using FaxSync.Models.FaxApi;
using System.Text;

namespace FaxSync.Domain
{
    public class ActionSync
    {
        public FaxApiUserActionSync FaxUserSyncObj { get; set; }
        public AssistantActionSync AssistantSnycObj { get; set; }
        public ActionSyncType ActionType { get; set; }
        public ActionSyncReason ActionReason { get; set; }
        public ApiResult Result  { get; set; }
        private ActionSync(ActionSyncType type, ActionSyncReason reason)
        {
            ActionType = type;
            ActionReason = reason;
        }
        public ActionSync(ActionSyncType type, ActionSyncReason reason, FaxApiUserActionSync faxUserObj) : this(type, reason)
        {
            FaxUserSyncObj = faxUserObj;
        }
        public ActionSync(ActionSyncType type, ActionSyncReason reason, string attorneyId, string userId, string faxNumber):this(type,reason)
        {
            AssistantSnycObj = new AssistantActionSync();          
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
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Action:{ActionType.ToString()} Reason:{ActionReason.ToString()}");
            if(ActionType == ActionSyncType.AddUser || ActionType == ActionSyncType.RemoveUser || ActionType == ActionSyncType.UpdateUser)
            {
                sb.Append($" User={FaxUserSyncObj.first_name} {FaxUserSyncObj.last_name} ({FaxUserSyncObj.username})");
            }
            else
            {
                sb.Append($" AttorneyId={AssistantSnycObj.AttorneyId}  AssistantUserId={AssistantSnycObj.FaxUserId} FaxNumber={AssistantSnycObj.FaxNumber}");
            }
            return sb.ToString();
        }
    }
}
