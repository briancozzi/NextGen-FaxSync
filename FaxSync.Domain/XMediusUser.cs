using FaxSync.Models;
using FaxSync.Models.FaxApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.Infrastructure.Utilities;
namespace FaxSync.Domain
{
    /// <summary>
    /// Assistantite mozda nemet main fax number , da vidam za polinjta mapping AD-FaxUser
    /// </summary>
    public class XMediusUser
    {
        public AdUser AdUser { get; set; }
        public FaxApiUser FaxUser { get; set; }
        public FaxApiGroup FaxGroup { get; set; }
        public FaxApiNumber FaxNumber { get; set; }
        public List<string> BlackListedFaxNumbers { get; set; }
        public bool IsBlackListedFaxNumber { get;set;}
        public List<ActionSync> ActionList { get; private set; } = new List<ActionSync>();
        public bool UserExsitsInXmedius { get
            {
                return FaxUser != null && FaxUser.username.CompareAreEqual(AdUser.UserId);
            }
        }
        public XMediusUser(AdUser adUser,List<FaxApiGroup> faxUserGroups,List<FaxApiUser>  faxUsers, List<FaxApiNumber> faxNumbers, List<string> blackListedFaxNumbers)
        {
            if (adUser == null) return;
            AdUser = adUser;
            BlackListedFaxNumbers = blackListedFaxNumbers;
            SetFaxUser(faxUsers);    
            SetFaxUserGroup(faxUserGroups);
            SetFaxNumber(faxNumbers);
            AnalyzeUserSyncActions();    
        }
        private void SetFaxNumber(List<FaxApiNumber> faxNumbers)
        {
            if (faxNumbers == null || AdUser == null || AdUser.FaxNumber.IsEmpty()) return;
            FaxNumber = faxNumbers?.FirstOrDefault(x => x.FaxNumber.CompareAreEqual(AdUser.FaxNumber));
        }
        private void SetFaxUser(List<FaxApiUser> faxUsers)
        {
            if (faxUsers == null || AdUser == null) return;
            FaxUser = faxUsers?.FirstOrDefault(x => x.username.CompareAreEqual(AdUser.UserId));
        }
        private void SetFaxUserGroup(List<FaxApiGroup> faxUserGroups)
        {
            if (faxUserGroups == null || AdUser==null) return;

            var officeName = AdUser.Office.IsNotEmpty() && AdUser.Office.ToLower().StartsWith("Washington") ? "Washington" : AdUser.Office;

            if (AdUser.Office.IsNotEmpty())
                FaxGroup = faxUserGroups.FirstOrDefault(x => x.Name.CompareAreEqual(AdUser.Office));
            else
                FaxGroup = faxUserGroups.FirstOrDefault(x => x.Default);
        }
        private void AnalyzeUserSyncActions()
        {
            MapAdUserToFaxApiUser();
            AnaylzeUserAddRemove();
            AnaylzeUserUpdate();
        }
        private void AnaylzeUserAddRemove()
        {
            if (AdUser == null) return;

            var userHasFaxAndOffice = AdUser.FaxNumber.IsNotEmpty() && AdUser.Office.IsNotEmpty();

            if (UserExsitsInXmedius.Not() && AdUser.Disabled.Not() && userHasFaxAndOffice) // user does not exsits need to be added  
            {
                this.AddActionSnycItem(ActionSyncType.AddUser, ActionSyncReason.NewUser, FaxUser);
            }
            else if (UserExsitsInXmedius && AdUser.Disabled) //user needs to be removed from XMedius
            {
                this.AddActionSnycItem(ActionSyncType.RemoveUser, ActionSyncReason.UserIsDisabled, FaxUser);
            }
            else if(UserExsitsInXmedius &&  userHasFaxAndOffice.Not())
            {
                this.AddActionSnycItem(ActionSyncType.RemoveUser, ActionSyncReason.UserWihtoutFaxAndOffice, FaxUser);
            }
        }
        private void AnaylzeUserUpdate()
        {
            if (AdUser.NotNull() || AdUser.Disabled || AdUser.FaxNumber.IsEmpty() || AdUser.Office.IsEmpty() ) return;

            if(UserExsitsInXmedius && FaxNumber?.FaxNumberId != FaxUser?.main_fax_number_id) // need to update fax
            {
                this.AddActionSnycItem(ActionSyncType.UpdateUser, ActionSyncReason.UserIsUpdated, FaxUser);
            }

            if(UserExsitsInXmedius && FaxGroup?.Id != FaxUser?.group_id) // needs to update group
            {
                this.AddActionSnycItem(ActionSyncType.UpdateUser, ActionSyncReason.UpdateUserFaxGroup, FaxUser);
            }
        }
        private void MapAdUserToFaxApiUser()
        {
            if(AdUser.NotNull() && UserExsitsInXmedius.Not())
            {
                FaxUser = new FaxApiUser();
                FaxUser.username = AdUser.UserId;
                FaxUser.first_name = AdUser.FirstName;
                FaxUser.last_name = AdUser.LastName;
                FaxUser.job_title = AdUser.JobTitle;
                FaxUser.phone_number = AdUser.PhoneNumber;
                FaxUser.mobile_number = AdUser.MobileNumber;
                FaxUser.main_fax_number = AdUser.FaxNumber;
                FaxUser.group_id = FaxGroup?.Id ?? 0;
                FaxUser.address = AdUser.Address;
                FaxUser.city = AdUser.City;
                FaxUser.email = AdUser.Email;
                FaxUser.external_id = "objectGUID";
                FaxUser.language = AdUser.Language;
                FaxUser.state = AdUser.State;
                FaxUser.main_fax_number_id = FaxNumber?.FaxNumberId ?? 0;
                FaxUser.role = "user";
            }
        }
        private void AddActionSnycItem(ActionSyncType type, ActionSyncReason reason, FaxApiUser user)
        {
            var faxSyncObject = new FaxSnyc.Models.Sync.FaxApiUserActionSync();
            ObjectMapper.PropertyMap(user, faxSyncObject);
            this.ActionList.Add(new ActionSync(type, reason, faxSyncObject));
        }
    }
}
