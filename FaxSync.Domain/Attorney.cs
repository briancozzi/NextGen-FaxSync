using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaxSync.Models;

namespace FaxSync.Domain
{
    public class Attorney
    {
        public User AttorneyUser { get; private set; }
        public User NewAssistant { get; private set; }
        public User CurrentAssistant { get; private set; }
        public User PreviousAssistant { get; private set; }
        public FaxNumber CurrentFaxNumber { get; private set; }
        public FaxNumber PreviousFaxNumber { get; private set; }
        public FaxNumber NewFaxNumber { get; private set; }
        public bool Excluded { get { return AttorneyUser.Excluded; } }
        public bool _faxChanged { get; private set; } = false;
        public bool _assistantChanged { get; private set; } = false;
        public List<ActionSync> ActionList { get; private set; } = new List<ActionSync>();
        public bool IsProccessed { get; private set; } = false;
        public Attorney(string AttorneyUserId, string AttorneyFullName, bool IsDisabled, bool Excluded)
        {
            this.AttorneyUser = new User()
            {

                UserId = AttorneyUserId,
                FullName = AttorneyFullName,
                Disabled = IsDisabled,
                Excluded = Excluded
            };
        }
        public void SetPreviousAssistant(string previousAssistantId, string previoustAssistantFullName)
        {
            PreviousAssistant = new User();
            SetAssistant(PreviousAssistant, previousAssistantId, previousAssistantId, false);
        }
        public void SetCurrentAssistant(string curentAssistantId, string curentAssistantFullName, bool IsDisabled)
        {
            CurrentAssistant = new User();
            SetAssistant(CurrentAssistant, curentAssistantId, curentAssistantFullName, IsDisabled);
        }
        public void SetNewAssistant(string newAssistantId, string newAssistantFullName, bool IsDisabled)
        {
            NewAssistant = new User();
            SetAssistant(NewAssistant, newAssistantId, newAssistantFullName, IsDisabled);
            _assistantChanged = CurrentAssistant.Compare(NewAssistant) != 0;
        }
        private void SetAssistant(User assistantObj, string assistantId, string assistantFullName, bool IsDisabled)
        {
            if (assistantId.IsEmpty())
            {
                assistantObj = null;
            }
            else
            {
                assistantObj = assistantObj ?? new User();
                assistantObj.UserId = assistantId;
                assistantObj.FullName = assistantFullName;
                assistantObj.Disabled = IsDisabled;
            }
        }
        public void SetFaxNumber(string peviousFaxNumber, string currentFaxNumber, List<string> blackListedFaxNumbers)
        {
            this.PreviousFaxNumber = new FaxNumber(peviousFaxNumber, blackListedFaxNumbers);
            this.CurrentFaxNumber = new FaxNumber(currentFaxNumber, blackListedFaxNumbers);
        }
        public void SetNewFaxNumber(string newFaxNumber, List<string> blackListedFaxNumbers)
        {
            this.NewFaxNumber = new FaxNumber(newFaxNumber, blackListedFaxNumbers);
            _faxChanged = CurrentFaxNumber.Compare(NewFaxNumber) != 0;
        }
        public void Process()
        {
            this.ActionList.Clear();
            IsProccessed = false;

            if (Excluded.Not() && AttorneyUser.Disabled.Not())
            {
                CheckAssistant();
                CheckFaxNumber();
            }

            IsProccessed = true;
        }
        private void CheckFaxNumber()
        {
            //Make sure that the fax is changed and is new fax is not Blacklisted
            if (_faxChanged.Not()) return;

            var newFaxNumberIsValid = NewFaxNumber.NotNull() && NewFaxNumber.Number.IsNotEmpty() && !NewFaxNumber.IsBlacklisted;

            if (CurrentAssistant.NotNull() && CurrentAssistant.UserId.IsNotEmpty())
            {
                if (CurrentFaxNumber.NotNull() && CurrentFaxNumber.Number.IsNotEmpty())
                {
                    AddActionSnycItem(ActionSyncType.RemoveUser, ActionSyncReason.FaxNumberChange, CurrentAssistant.UserId, CurrentFaxNumber.Number);
                }

                if (newFaxNumberIsValid)
                {
                    if (_assistantChanged.Not())
                    {
                        AddActionSnycItem(ActionSyncType.AddUser, ActionSyncReason.FaxNumberChange, CurrentAssistant.UserId, NewFaxNumber.Number);
                    }

                }
            }

            if (_assistantChanged && NewAssistant.NotNull() && NewAssistant.UserId.IsNotEmpty())
            {
                if (newFaxNumberIsValid)
                {
                    AddActionSnycItem(ActionSyncType.AddUser, ActionSyncReason.FaxAndAssistantChange, NewAssistant.UserId, NewFaxNumber.Number);
                }
            }
        }
        private void CheckAssistant()
        {
            if (_assistantChanged.Not()) return;

            if (_faxChanged.Not() && CurrentFaxNumber.NotNull() && CurrentFaxNumber.Number.IsNotEmpty() && CurrentFaxNumber.IsBlacklisted.Not())
            {
                if (CurrentAssistant.NotNull() && CurrentAssistant.UserId.IsNotEmpty())
                {
                    AddActionSnycItem(ActionSyncType.RemoveUser, ActionSyncReason.AssistantChange, CurrentAssistant.UserId, CurrentFaxNumber.Number);
                }

                if (NewAssistant.NotNull() && NewAssistant.UserId.IsNotEmpty())
                {
                    AddActionSnycItem(ActionSyncType.AddUser, ActionSyncReason.AssistantChange, NewAssistant.UserId, CurrentFaxNumber.Number);
                }
            }



        }
        private ActionSyncReason GiveMeTheRason()
        {
            if (_faxChanged && _assistantChanged)
                return ActionSyncReason.FaxAndAssistantChange;

            if (_faxChanged)
                return ActionSyncReason.FaxNumberChange;
            else
                return ActionSyncReason.AssistantChange;

        }
        private void AddActionSnycItem(ActionSyncType type, ActionSyncReason reason, string userId, string faxNumber)
        {
            this.ActionList.Add(new ActionSync(type, reason,AttorneyUser.UserId, userId, faxNumber));
        }
        public DbUser GetUserChanges()
        {
            var _dbUser = new DbUser();
            _dbUser.AttorneyUserID = AttorneyUser.UserId;
            
            if(_faxChanged)
            {
                _dbUser.PreviousFaxNumber = CurrentFaxNumber?.Number;
                _dbUser.CurrentFaxNumber = NewFaxNumber?.Number;
            }
            else
            {
                _dbUser.PreviousFaxNumber = PreviousFaxNumber?.Number;
                _dbUser.CurrentFaxNumber = CurrentFaxNumber?.Number;
            }

            if(_assistantChanged)
            {
                _dbUser.PreviousAssistantUserId = CurrentAssistant?.UserId;
                _dbUser.CurrentAssistantUserId = NewAssistant?.UserId;
            }
            else
            {
                _dbUser.PreviousAssistantUserId = PreviousAssistant?.UserId;
                _dbUser.CurrentAssistantUserId = CurrentAssistant?.UserId;
            }
            
            if(_assistantChanged || _faxChanged)
            {
                _dbUser.DateUpdated = DateTime.Now;
            }
            

            return _dbUser;
        }
        public List<LogResult> GetLogChanges()
        {
            if (ActionList.NotNull() && !ActionList.Any()) return new List<LogResult>();

            var _lstLogResult = new List<LogResult>();
            var attorneyId = AttorneyUser.UserId;
            

            if (_faxChanged)
            {
                var _logResult = new LogResult();
                _logResult.UserId = attorneyId;
                _logResult.Type = LogType.Audit;
                _logResult.FieldName = nameof(CurrentFaxNumber);
                _logResult.PreviousValue = CurrentFaxNumber?.Number;
                _logResult.NewValue = NewFaxNumber?.Number;
                _logResult.Result = true;
                _logResult.Message = "Fax Changed";
                _lstLogResult.Add(_logResult);
            }
            if(_assistantChanged)
            {
                var _logResult = new LogResult();
                _logResult.UserId = attorneyId;
                _logResult.Type = LogType.Audit;
                _logResult.FieldName = nameof(CurrentAssistant);
                _logResult.PreviousValue = CurrentAssistant?.UserId;
                _logResult.NewValue = NewAssistant?.UserId;
                _logResult.Result = true;
                _logResult.Message = "Assistant Changed";
                _lstLogResult.Add(_logResult);
            }

            foreach (var action in ActionList)
            {
                if(action.Result.NotNull())
                {
                    var _logResult = new LogResult();
                    _logResult.UserId = attorneyId;
                    _logResult.Type = LogType.ApiCall;
                    _logResult.Result = action.Result.Result;
                    _logResult.FieldName = "FaxSoultion";
                    _logResult.NewValue = $"AttId:{action.FaxAttorneyUserId} UsrId:{action.FaxUserId} FaxId:{action.FaxNumberId}";
                    _logResult.Message = $"Action:{action.ActionType.ToString()} Message:{action.Result.HttpCallLog} {action.Result.Message}";
                    _lstLogResult.Add(_logResult);
                }
            }

            return _lstLogResult;
        }


    }

}
