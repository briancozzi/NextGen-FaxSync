﻿using FaxSync.Domain;
using FaxSync.Models;
using FaxSync.Models.FaxApi;
using FaxSync.Models.Interface;
using FaxSync.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services
{
    public class SyncAssistantService : SyncService, IFaxSyncService
    {   
        public void Sync(int? logSessionId = null)
        {
            var sessionId = LogService.LogStartSession();
            LogService.LogStartEvent("START - xMedius Script - Sync Assistants Script");
            SyncMissingDbUsers();
            var listAttorney = LoadAttorneys();
            var listOfExecutionResults = SyncUsersWithFaxSolution(listAttorney);
            UpdateDbUsersFromExecutionResults(listOfExecutionResults);
            LogChanges(sessionId, listOfExecutionResults);
            LogService.LogEndSession(sessionId, result: true, message: "Result message");
            DirectoryInfo directory = new DirectoryInfo(@"C:\xMediusSyncApp\Logs");
            directory.Create();
            File.WriteAllLines("C:\\xMediusSyncApp\\Logs\\XMedius_SyncAssistants_Log_" + DateTime.Now.ToString("yyyyMMddHHmmssffff"), LogService.LogEventsList);

        }

        private void LogChanges(int sessionId, Dictionary<Attorney, bool> listOfExecutionResults)
        {
            this.LogService.LogEvent("START: Log changes");
            var lstLogs = GetLogChanges(listOfExecutionResults, sessionId);
            LogService.Log(lstLogs);
            this.LogService.LogEvent("FINISH: Log changes");
        }
        private List<LogResult> GetLogChanges(Dictionary<Attorney, bool> listOfExecutionResults, int sessionId)
        {
            var lstLogs = new List<LogResult>();
            foreach (var result in listOfExecutionResults)
            {
                var attroneyObj = result.Key;
                lstLogs.AddRange(attroneyObj.GetLogChanges());
            }
            foreach (var logItem in lstLogs)
            {
                logItem.SessionId = sessionId;
            }
            return lstLogs;
        }
        private void SyncMissingDbUsers()
        {
            this.LogService.LogEvent("START: Sync missing users in was/is table");
            var adUsers = AdUserService.GetCachedUsers();
            var dbUsers = DbUserService.GetAllUsers();
            var dbUsersIds = dbUsers.Select(x => x.AttorneyUserID.ToLower()).ToList();
            var missingUsers = adUsers.Where(x => !dbUsersIds.Contains(x.UserId.ToLower())).ToList();
            DbUserService.AddMissingUsers(missingUsers);
            this.LogService.LogEvent("FINISH: Sync missing users in was/is table");
        }
        private List<Attorney> LoadAttorneys()
        {
            this.LogService.LogEvent("START: Load Attorneys: Loading the data from AD and mapping with DB user.");
            var dbUsers = DbUserService.GetAllUsers();
            var blackListedNumbers = HelperService.GetBlackListedFaxNumbers();

            var listAttoryes = new List<Attorney>();
            foreach (var user in dbUsers)
            {
                var attorneyObj = BuildAttorney(user, blackListedNumbers);
                if(attorneyObj.NotNull())
                    listAttoryes.Add(attorneyObj);

            }
            this.LogService.LogEvent("FINISH: Load Attorneys: Loading the data from AD and mapping with DB user.");
            return listAttoryes;
        }
        private Attorney BuildAttorney(IDbUser dbUser, List<string> blackListedFaxNumbers)
        {
            var attorney = AdUserService.GetUserById(dbUser.AttorneyUserID);
            if (attorney == null)
                return null;

            var previousAssistant = AdUserService.GetUserById(dbUser.PreviousAssistantUserId);
            var currentAssistant = AdUserService.GetUserById(dbUser.CurrentAssistantUserId);
            var newAssistant = AdUserService.GetUserById(attorney.AssistantId);

            var attorneyObject = new Attorney(attorney.UserId, attorney.DisplayName, attorney.Disabled, attorney.Excluded);

            if (dbUser.PreviousAssistantUserId.IsNotEmpty())
            {
                attorneyObject.SetPreviousAssistant(previousAssistant?.UserId ?? dbUser.PreviousAssistantUserId, previousAssistant?.DisplayName ?? "");
            }

            if (dbUser.CurrentAssistantUserId.IsNotEmpty())
            {
                attorneyObject.SetCurrentAssistant(currentAssistant?.UserId ?? dbUser.CurrentAssistantUserId, currentAssistant?.DisplayName ?? "", currentAssistant?.Disabled ?? false);
            }

            if (newAssistant != null)
            {
                attorneyObject.SetNewAssistant(newAssistant.UserId, newAssistant.DisplayName, newAssistant.Disabled);
            }

            attorneyObject.SetFaxNumber(dbUser.PreviousFaxNumber, dbUser.CurrentFaxNumber, blackListedFaxNumbers);
            attorneyObject.SetNewFaxNumber(attorney.FaxNumber, blackListedFaxNumbers);
            attorneyObject.Process();

            return attorneyObject;

        }
        private Dictionary<Attorney, bool> SyncUsersWithFaxSolution(List<Attorney> listAttorney)
        {
            this.LogService.LogEvent("START: Getting data from FAX Solution, API calls to get FaxNumbers and FaxUsers");
            var faxNumbers = FaxApiService.GetAllFaxNumbers().data;
            var faxUsers = FaxApiService.GetAllUsers().data;
            var listOfExecution = new Dictionary<Attorney, bool>();
            this.LogService.LogEvent("FINISH: Getting data from FAX Solution, API calls to get FaxNumbers and FaxUsers");
            this.LogService.LogEvent("START: MAIN Processing the API calls to the FAX Solution to sync data.");
            foreach (var attr in listAttorney)
            {
                if (attr.ActionList.NotNull() && attr.ActionList.Count() > 0)
                {
                    var apiSuccess = ProcessActionItems(attr.ActionList, faxUsers, faxNumbers);
                    listOfExecution.Add(attr, apiSuccess);
                }
            }
            this.LogService.LogEvent("FINISH: MAIN Processing the API calls to the FAX Solution to sync data.");
            return listOfExecution;
        }
        private bool ProcessActionItems(List<ActionSync> actionItems, List<FaxApiUser> faxUsers, List<FaxApiNumber> faxNumbers)
        {

            if (actionItems.NotNull() && actionItems.Count() > 0)
            {
                foreach (var action in actionItems)
                {
                    MapFaxSolutionIdsWithActionItems(action, faxUsers, faxNumbers);

                    if (action.ActionType == ActionSyncType.AssignUser && action.AssistantSnycObj.FaxNumberIsShared.Not())
                        action.Result = FaxApiService.AssignUser(action.AssistantSnycObj.FaxNumberId, action.AssistantSnycObj.FaxUserId);
                    else if (action.ActionType == ActionSyncType.DeAssignUser)
                        action.Result = FaxApiService.UnAssignUser(action.AssistantSnycObj.FaxNumberId, action.AssistantSnycObj.FaxUserId);
                }
            }

            return actionItems.NotNull() && actionItems.Where(x => x.Result.Result.Not()).Count() == 0;
        }
        private void MapFaxSolutionIdsWithActionItems(ActionSync item, List<FaxApiUser> faxUsers, List<FaxApiNumber> faxNumbers)
        {
            item.AssistantSnycObj.FaxAttorneyUserId = faxUsers?.Where(x => item.AssistantSnycObj.AttorneyId.CompareAreEqual(x.username))
                                             .FirstOrDefault()?.id ?? 0;
            var faxNumObj = faxNumbers?.Where(x => item.AssistantSnycObj.FaxNumber.CompareAreEqual(x.FaxNumber) && x.Shared.Not())
                                         .FirstOrDefault();
            item.AssistantSnycObj.FaxNumberId = faxNumObj?.FaxNumberId ?? 0;
            item.AssistantSnycObj.FaxNumberIsShared = faxNumObj?.Shared ?? false;
            item.AssistantSnycObj.FaxUserId = faxUsers?.Where(x => item.AssistantSnycObj.UserId.CompareAreEqual(x.username))
                                     .FirstOrDefault()?.id ?? 0;
            if (item.AssistantSnycObj.FaxAttorneyUserId == 0 || item.AssistantSnycObj.FaxNumberId == 0 || item.AssistantSnycObj.FaxUserId == 0)
            {
                this.LogService.LogEvent($"ERROR: Attorney:{item.AssistantSnycObj.AttorneyId} User:{item.AssistantSnycObj.UserId} Fax:{item.AssistantSnycObj.FaxNumber} FaxId:{item.AssistantSnycObj.FaxNumberId} FaxUserId:{item.AssistantSnycObj.FaxUserId}");
            }
        }
        private void UpdateDbUsersFromExecutionResults(Dictionary<Attorney, bool> listOfExecutionResults)
        {
            this.LogService.LogEvent("START: Update Database with Sync Results - was/is.");
            var listSuccesfulyUpdatedUsers = new List<IDbUser>();
            foreach (var result in listOfExecutionResults)
            {
                var sucessfulyExecuted = result.Value;
                if (sucessfulyExecuted)
                {
                    var dbUser = BuildDbUserFromAttorney(result.Key);
                    listSuccesfulyUpdatedUsers.Add(dbUser);
                }
            }

            DbUserService.UpdateUsers(listSuccesfulyUpdatedUsers);
            this.LogService.LogEvent("FINISH: Update Database with Sync Results - was/is.");
        }
        private IDbUser BuildDbUserFromAttorney(Attorney attorney)
        {
            var dbUser = attorney.GetUserChanges();
            return dbUser;
        }

    }
}
