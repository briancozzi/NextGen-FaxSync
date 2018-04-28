using FaxSync.Models;
using FaxSync.Models.FaxApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool UserExsitsInXmedius { get
            {
                return FaxUser != null && FaxUser.username.CompareAreEqual(AdUser.UserId);
            }
        }
        public XMediusUser(AdUser adUser,List<FaxApiGroup> faxUserGroups,List<FaxApiUser>  faxUsers, List<FaxApiNumber> faxNumbers)
        {
            AdUser = adUser;
            SetFaxUser(faxUsers);
            SetFaxUserGroup(faxUserGroups);
            SetFaxNumber(faxNumbers);
            AnalyzeUserSyncActions();    
        }
        private void SetFaxNumber(List<FaxApiNumber> faxNumbers)
        {
            if (faxNumbers == null || AdUser == null) return;
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

            FaxGroup = faxUserGroups.FirstOrDefault(x => x.Name.CompareAreEqual(AdUser.Office));
        }
        private void AnalyzeUserSyncActions()
        {
            AnaylzeUserAddRemove();
            AnaylzeUserUpdate();
        }
        private void AnaylzeUserAddRemove()
        {
            if (AdUser == null) return;
            if (UserExsitsInXmedius.Not() && AdUser.Disabled.Not()) // user does not exsits need to be added  
            { }
            else if (UserExsitsInXmedius && AdUser.Disabled) //user needs to be removed from XMedius
            { }
        }
        private void AnaylzeUserUpdate()
        {
            if (AdUser.Disabled) return;
            if(UserExsitsInXmedius && FaxNumber.FaxNumberId != FaxUser.main_fax_number_id) // need to update fax
            {}

            if(UserExsitsInXmedius && FaxGroup.Id != FaxUser.group_id) // needs to update group
            { }
        }
        private void MapAdUserToFaxApiUser()
        {
            if(AdUser.NotNull() && UserExsitsInXmedius.Not())
            {
                FaxUser = new FaxApiUser();
                FaxUser.username = AdUser.UserId;
                FaxUser.first_name = AdUser.FirstName;
                FaxUser.last_name = AdUser.LastName;
                FaxUser.role = AdUser.Role;
                FaxUser.job_title = AdUser.JobTitle;
                FaxUser.phone_number = AdUser.PhoneNumber;
                FaxUser.mobile_number = AdUser.MobileNumber;

                FaxUser.group_id = FaxGroup?.Id ?? 0;
            }
        }
    }
}
