using FaxSync.Domain;
using FaxSync.Models;
using FaxSync.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services
{
    public class SyncUserService : SyncService, IFaxSyncService
    {
        public void Sync(int? logSessionId = null)
        {
            LogService.LogStartEvent("START - xMedius Script - Sync Users Script");
            LogService.LogEvent("Pulling users from Active Directory");
            var adUsers = AdUserService.GetCachedUsers();
            LogService.LogEvent("Pulling data from xMedius - Users,Groups,FaxNumbeers");
            var blackListedNumbers = HelperService.GetBlackListedFaxNumbers();
            var faxNumbers = FaxApiService.GetAllFaxNumbers().data;
            var faxUsers = FaxApiService.GetAllUsers().data;
            var faxGroups = FaxApiService.GetAllFaxGroups().data;
            var lstXMediusDomainObj = new List<XMediusUser>();
            LogService.LogEvent("Analyzing data...");
            foreach (var usr in adUsers)
            {
                var buildUser = new XMediusUser(usr as AdUser, faxGroups, faxUsers, faxNumbers, blackListedNumbers);
                lstXMediusDomainObj.Add(buildUser);        
            }

            var usersWithActions = lstXMediusDomainObj.Where(x => x.ActionList.Any()).ToList();
            var disabledUsers = usersWithActions.Where(x => x.AdUser.Disabled).ToList();
            var updatedUsers = usersWithActions.Where(x => x.ActionList.Where(y => y.ActionReason == ActionSyncReason.UpdateUserFaxGroup).Any()).ToList();
            var updatedUsersFax = usersWithActions.Where(x => x.ActionList.Where(y => y.ActionReason == ActionSyncReason.UserIsUpdated).Any()).ToList();
            var userWithouFaxAndOFfice = usersWithActions.Where(x => x.ActionList.Where(y => y.ActionReason == ActionSyncReason.UserWihtoutFaxAndOffice).Any()).ToList();
            var restOfUsersNew = usersWithActions.Where(x => x.ActionList.Where(y => y.ActionType == ActionSyncType.AddUser).Any()).ToList();
            var restOfUsersRemove = usersWithActions.Where(x => x.ActionList.Where(y => y.ActionType == ActionSyncType.RemoveUser).Any()).ToList();
            var restOfUsersUpdate = usersWithActions.Where(x => x.ActionList.Where(y => y.ActionType == ActionSyncType.UpdateUser).Any()).ToList();

            LogService.LogEvent($"Stats - AddUsers:{restOfUsersNew.Count()} RemoveUsers:{restOfUsersRemove.Count()} UpdateUsers:{restOfUsersUpdate.Count()}",false);

            restOfUsersNew.ForEach(x => x.ActionList.ForEach( y=> LogService.LogEvent(y.ToString())));
            restOfUsersUpdate.ForEach(x => x.ActionList.ForEach(y => LogService.LogEvent(y.ToString())));
            restOfUsersRemove.ForEach(x => x.ActionList.ForEach(y => LogService.LogEvent(y.ToString())));

            LogService.LogEvent("Sync DONE!");

            File.WriteAllLines("XMedius_SyncUsers_Log_" + DateTime.Now.ToString("yyyyMMddHHmmssffff"), LogService.LogEventsList);

        }
    }
}
