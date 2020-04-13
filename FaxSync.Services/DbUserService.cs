using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaxSync.Services.Interface;
using FaxSync.Models.Interface;
using FaxSync.Models;
using FaxSync.DataAccess;
using NextGen.Infrastructure.Utilities;

namespace FaxSync.Services
{
    public class DbUserService : IDbUserService
    {
        private XMediusFaxSyncContext xMediusContext { get; set; } 
        public DbUserService()
        {
        }
        
        public bool AddMissingUsers(IEnumerable<IAdUser> users)
        {
            xMediusContext = new XMediusFaxSyncContext();
            var usersIdList = users.Select(x => x.UserId).ToList();
            var usersThatExsits = xMediusContext.XMediusFaxAssistantSyncs
                                                .Where(x => usersIdList.Contains(x.AttorneyUserID)).Select(x => x.AttorneyUserID).ToList();
            var usetNotExsits = users.Where(x => !usersThatExsits.Contains(x.UserId)).ToList();


            var dbUsers = usetNotExsits.Select(x => new XMediusFaxAssistantSync()
            {
                AttorneyUserID = x.UserId,
                DateCreated = DateTime.Now
            });
            xMediusContext.XMediusFaxAssistantSyncs.AddRange(dbUsers);
            xMediusContext.SaveChanges();
            return true;
        }

        public bool UpdateUser(IDbUser user)
        {
            xMediusContext = new XMediusFaxSyncContext();
            var dbUser = xMediusContext.XMediusFaxAssistantSyncs.FirstOrDefault(x => x.AttorneyUserID == x.AttorneyUserID);
           
            MapUserForUpdateding(user, dbUser);
            xMediusContext.SaveChanges();
            return true;
        }
        public bool UpdateUsers(IEnumerable<IDbUser> user)
        {
            xMediusContext = new XMediusFaxSyncContext();
            foreach (var usr in user)
            {
                var dbUser = xMediusContext.XMediusFaxAssistantSyncs.FirstOrDefault(x => x.AttorneyUserID == usr.AttorneyUserID);
                MapUserForUpdateding(usr, dbUser);                   
            }
            xMediusContext.SaveChanges();
            return true;
        }

        public IEnumerable<IDbUser> GetAllUsers()
        {
            xMediusContext = new XMediusFaxSyncContext();
            var listDbUsers = new List<IDbUser>();
            xMediusContext.XMediusFaxAssistantSyncs.ToList().ForEach(x =>
            {
                var dbUserObj = new DbUser();
                ObjectMapper.PropertyMap(x, dbUserObj);
                listDbUsers.Add(dbUserObj);
            });
            return listDbUsers;
        }

        private void MapUserForUpdateding(IDbUser user, XMediusFaxAssistantSync dbUser)
        {
            if (dbUser == null) return;
            dbUser.CurrentAssistantUserId = user.CurrentAssistantUserId;
            dbUser.CurrentFaxNumber = user.CurrentFaxNumber;
            dbUser.DateUpdated = DateTime.Now;
            dbUser.PreviousAssistantUserId = user.PreviousAssistantUserId;
            dbUser.PreviousFaxNumber = user.PreviousFaxNumber;
        }


    }
}
