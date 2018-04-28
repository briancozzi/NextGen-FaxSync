using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FaxSync.Domain;
using System.Collections.Generic;
using FaxSync.Models;

namespace FaxSync.UnitTest
{
    [TestClass]
    public class SyncTest
    {
        List<string> blackListNumbers;
        Dictionary<string, User> usersList;
        [TestInitialize]
        public void Init()
        {
            blackListNumbers = new List<string>();
            blackListNumbers.Add("123456789");
            blackListNumbers.Add("987654321");

            InitUserList();
        }
        [TestMethod]
        public void NewAttorney()
        {
            var attorney = InitAttorney();
            var userNewAssistant = usersList["na"];
            attorney.SetNewAssistant(userNewAssistant.UserId, userNewAssistant.FullName, userNewAssistant.Disabled);
            attorney.SetNewFaxNumber("7739835668", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.ActionList.Count == 1);
            Assert.IsTrue(attorney.ActionList[0].ActionType == ActionSyncType.AssignUser);
            Assert.IsTrue(attorney.ActionList[0].ActionReason == ActionSyncReason.FaxAndAssistantChange);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.UserId == userNewAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.FaxNumber == attorney.NewFaxNumber.Number);
        }
        [TestMethod]
        public void NewAttorneyBlackListedNumber()
        {
            var attorney = InitAttorney();
            var userNewAssistant = usersList["na"];
            attorney.SetNewAssistant(userNewAssistant.UserId, userNewAssistant.FullName, userNewAssistant.Disabled);
            attorney.SetNewFaxNumber("123456789", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.NewFaxNumber.IsBlacklisted);
            Assert.IsTrue(attorney.ActionList.Count == 0);
        }
        [TestMethod]
        public void AttorneyChangeAssistant()
        {
            var attorney = InitAttorney();
            var userPreviousAssistant = usersList["pa"];
            var userCurrentAssistant = usersList["ca"];
            var userNewAssistant = usersList["na"];
            attorney.SetPreviousAssistant(userPreviousAssistant.UserId, userPreviousAssistant.FullName);
            attorney.SetCurrentAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetNewAssistant(userNewAssistant.UserId, userNewAssistant.FullName, userNewAssistant.Disabled);
            attorney.SetFaxNumber("", "7739835668", blackListNumbers);
            attorney.SetNewFaxNumber("7739835668", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.ActionList.Count == 2);
            Assert.IsTrue(attorney.ActionList[0].ActionType == ActionSyncType.DeAssignUser);
            Assert.IsTrue(attorney.ActionList[0].ActionReason == ActionSyncReason.AssistantChange);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.UserId == userCurrentAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.FaxNumber == attorney.CurrentFaxNumber.Number);

            Assert.IsTrue(attorney.ActionList[1].ActionType == ActionSyncType.AssignUser);
            Assert.IsTrue(attorney.ActionList[1].ActionReason == ActionSyncReason.AssistantChange);
            Assert.IsTrue(attorney.ActionList[1].AssistantSnycObj.UserId == userNewAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[1].AssistantSnycObj.FaxNumber == attorney.CurrentFaxNumber.Number);
        }
        [TestMethod]
        public void AttorneyRemoveAssistant()
        {
            var attorney = InitAttorney();
            var userPreviousAssistant = usersList["pa"];
            var userCurrentAssistant = usersList["ca"];
            var userNewAssistant = usersList["na"];
            attorney.SetPreviousAssistant(userPreviousAssistant.UserId, userPreviousAssistant.FullName);
            attorney.SetCurrentAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetNewAssistant("","", false);
            attorney.SetFaxNumber("", "7739835668", blackListNumbers);
            attorney.SetNewFaxNumber("7739835668", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.ActionList.Count == 1);
            Assert.IsTrue(attorney.ActionList[0].ActionType == ActionSyncType.DeAssignUser);
            Assert.IsTrue(attorney.ActionList[0].ActionReason == ActionSyncReason.AssistantChange);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.UserId == userCurrentAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.FaxNumber == attorney.CurrentFaxNumber.Number);
        }
        [TestMethod]
        public void AttorneyChangeFaxNumber()
        {
            var attorney = InitAttorney();
            var userPreviousAssistant = usersList["pa"];
            var userCurrentAssistant = usersList["ca"];

            attorney.SetPreviousAssistant(userPreviousAssistant.UserId, userPreviousAssistant.FullName);
            attorney.SetCurrentAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetNewAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetFaxNumber("", "7739835668", blackListNumbers);
            attorney.SetNewFaxNumber("7739831234", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.ActionList.Count == 2);
            Assert.IsTrue(attorney.ActionList[0].ActionType == ActionSyncType.DeAssignUser);
            Assert.IsTrue(attorney.ActionList[0].ActionReason == ActionSyncReason.FaxNumberChange);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.UserId == userCurrentAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.FaxNumber == attorney.CurrentFaxNumber.Number);

            Assert.IsTrue(attorney.ActionList[1].ActionType == ActionSyncType.AssignUser);
            Assert.IsTrue(attorney.ActionList[1].ActionReason == ActionSyncReason.FaxNumberChange);
            Assert.IsTrue(attorney.ActionList[1].AssistantSnycObj.UserId == userCurrentAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[1].AssistantSnycObj.FaxNumber == attorney.NewFaxNumber.Number);
        }
        [TestMethod]
        public void AttorneyChangeFaxNumberAndAssistant()
        {
            var attorney = InitAttorney();
            var userPreviousAssistant = usersList["pa"];
            var userCurrentAssistant = usersList["ca"];
            var userNewAssistant = usersList["na"];

            attorney.SetPreviousAssistant(userPreviousAssistant.UserId, userPreviousAssistant.FullName);
            attorney.SetCurrentAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetNewAssistant(userNewAssistant.UserId, userNewAssistant.FullName, userNewAssistant.Disabled);
            attorney.SetFaxNumber("", "7739835668", blackListNumbers);
            attorney.SetNewFaxNumber("7739831234", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.ActionList.Count == 2);
            Assert.IsTrue(attorney.ActionList[0].ActionType == ActionSyncType.DeAssignUser);
            Assert.IsTrue(attorney.ActionList[0].ActionReason == ActionSyncReason.FaxNumberChange);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.UserId == userCurrentAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.FaxNumber == attorney.CurrentFaxNumber.Number);

            Assert.IsTrue(attorney.ActionList[1].ActionType == ActionSyncType.AssignUser);
            Assert.IsTrue(attorney.ActionList[1].ActionReason == ActionSyncReason.FaxAndAssistantChange);
            Assert.IsTrue(attorney.ActionList[1].AssistantSnycObj.UserId == userNewAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[1].AssistantSnycObj.FaxNumber == attorney.NewFaxNumber.Number);
        }
        [TestMethod]
        public void AttorneyChangeFaxNumberAndAssistantWithBlackListedFax()
        {
            var attorney = InitAttorney();
            var userPreviousAssistant = usersList["pa"];
            var userCurrentAssistant = usersList["ca"];
            var userNewAssistant = usersList["na"];

            attorney.SetPreviousAssistant(userPreviousAssistant.UserId, userPreviousAssistant.FullName);
            attorney.SetCurrentAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetNewAssistant(userNewAssistant.UserId, userNewAssistant.FullName, userNewAssistant.Disabled);
            attorney.SetFaxNumber("", "7739835668", blackListNumbers);
            attorney.SetNewFaxNumber("123456789", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.ActionList.Count == 1);
            Assert.IsTrue(attorney.ActionList[0].ActionType == ActionSyncType.DeAssignUser);
            Assert.IsTrue(attorney.ActionList[0].ActionReason == ActionSyncReason.FaxNumberChange);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.UserId == userCurrentAssistant.UserId);
            Assert.IsTrue(attorney.ActionList[0].AssistantSnycObj.FaxNumber == attorney.CurrentFaxNumber.Number);
        }
        [TestMethod]
        public void AttorneyChangeFaxNumberFromBlackListedToBlackListed()
        {
            var attorney = InitAttorney();
            var userPreviousAssistant = usersList["pa"];
            var userCurrentAssistant = usersList["ca"];

            attorney.SetPreviousAssistant(userPreviousAssistant.UserId, userPreviousAssistant.FullName);
            attorney.SetCurrentAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetNewAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetFaxNumber("", "123456789", blackListNumbers);
            attorney.SetNewFaxNumber("123456789", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.ActionList.Count == 0);
        }
        [TestMethod]
        public void AttorneyChangeFaxNumberAndAssistantWithFromToBlackListedFax()
        {
            var attorney = InitAttorney();
            var userPreviousAssistant = usersList["pa"];
            var userCurrentAssistant = usersList["ca"];
            var userNewAssistant = usersList["na"];

            attorney.SetPreviousAssistant(userPreviousAssistant.UserId, userPreviousAssistant.FullName);
            attorney.SetCurrentAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetNewAssistant(userNewAssistant.UserId, userNewAssistant.FullName, userNewAssistant.Disabled);
            attorney.SetFaxNumber("", "123456789", blackListNumbers);
            attorney.SetNewFaxNumber("123456789", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.ActionList.Count == 0);          
        }
        [TestMethod]
        public void ExcludedAttorney()
        {
            var attorney = InitAttorney(excluded: true);
            var userPreviousAssistant = usersList["pa"];
            var userCurrentAssistant = usersList["ca"];

            attorney.SetPreviousAssistant(userPreviousAssistant.UserId, userPreviousAssistant.FullName);
            attorney.SetCurrentAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetNewAssistant(userCurrentAssistant.UserId, userCurrentAssistant.FullName, userCurrentAssistant.Disabled);
            attorney.SetFaxNumber("", "7739835668", blackListNumbers);
            attorney.SetNewFaxNumber("7739831234", blackListNumbers);

            attorney.Process();

            Assert.IsTrue(attorney.Excluded);
            Assert.IsTrue(attorney.ActionList.Count == 0);
        }
        [TestMethod]
        public void AttorneyProcessed()
        {
            var attorney = InitAttorney(excluded: true);
           
            attorney.Process();

            Assert.IsTrue(attorney.IsProccessed);
        }
        [TestMethod]
        public void AttorneyNotProcessed()
        {
            var attorney = InitAttorney(excluded: true);

            Assert.IsFalse(attorney.IsProccessed);
        }

        #region "Helper classes"

        private Attorney InitAttorney(bool excluded = false)
        {
            var userAttorney = usersList["a"];
            return new Attorney(userAttorney.UserId, userAttorney.FullName, userAttorney.Disabled, excluded);    
        }
        private void InitUserList()
        {
            var userAttorney = new User
            {
                UserId = "marko",
                FullName = "Marko Mitreski",
                Disabled = false,
                Excluded = false
            };
            var userPreviousAssistant = new User
            {
                UserId = "beti",
                FullName = "Beti Mitreski",
                Disabled = false,
            };
            var userCurrentAssistant = new User
            {
                UserId = "zore",
                FullName = "Zore Mitreski",
                Disabled = false,
            };

            var userNewAssistant = new User
            {
                UserId = "andriana",
                FullName = "Andriana Mitreski",
                Disabled = false,
            };

            usersList = new Dictionary<string, User>();
            usersList.Add("a", userAttorney);
            usersList.Add("pa", userPreviousAssistant);
            usersList.Add("ca", userCurrentAssistant);
            usersList.Add("na", userNewAssistant);
        }
        #endregion
    }
}
