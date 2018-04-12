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
        public string AttorneyId { get; set; }
        public string UserId { get; set; }
        public string FaxNumber { get; set; }
        public int FaxAttorneyUserId { get; set; }
        public int FaxUserId { get; set; }
        public int FaxNumberId { get; set; }
        public bool FaxNumberIsShared { get; set; }
        public ActionSyncType ActionType { get; set; }
        public ActionSyncReason ActionReason { get; set; }
        public ApiResult Result  { get; set; }
        public ActionSync(ActionSyncType type, ActionSyncReason reason, string attorneyId, string userId, string faxNumber)
        {
            ActionType = type;
            ActionReason = reason;
            AttorneyId = attorneyId;
            UserId = userId;
            FaxNumber = faxNumber;
        }
        public void SetFaxSolutionsIds(int faxAttorneyUserId, int faxUserId, int faxNumberId)
        {
            FaxAttorneyUserId = faxAttorneyUserId;
            FaxNumberId = faxNumberId;
            FaxUserId = faxUserId;
        }
    }
}
