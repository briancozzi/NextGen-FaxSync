using FaxSnyc.Models.Sync;
using FaxSync.Models;
using FaxSync.Models.FaxApi;
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
    }
}
