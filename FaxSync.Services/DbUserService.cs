using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaxSync.Services.Interface;
using FaxSync.Models.Interface;
using FaxSync.Models;

namespace FaxSync.Services
{
    public class DbUserService : IDbUserService
    {
        List<DbUser> _listUsers;
        public DbUserService()
        {
            _listUsers = new List<DbUser>();
            //var user = new DbUser();
            //user.AttorneyUserID = "pkuchnicki";
            //user.CurrentAssistantUserId = "";
            ////user.CurrentFaxNumber = "+14123942555";
            //_listUsers.Add(user);
        }
        
        public bool AddMissingUsers(IEnumerable<IAdUser> user)
        {
            var dbUsers = user.Select(x => new DbUser()
            {
                AttorneyUserID = x.UserId,
                DateCreated = DateTime.Now
            });
            _listUsers.AddRange(dbUsers);
            return true;
        }

        public bool UpdateUser(IDbUser user)
        {
            var dbUser = _listUsers.FirstOrDefault(x => x.AttorneyUserID == x.AttorneyUserID);
            dbUser.CurrentAssistantUserId = user.CurrentAssistantUserId;
            dbUser.CurrentFaxNumber = user.CurrentFaxNumber;
            dbUser.DateUpdated = DateTime.Now;
            dbUser.PreviousAssistantUserId = user.PreviousAssistantUserId;
            dbUser.PreviousFaxNumber = user.PreviousFaxNumber;
            return true;
        }
        public bool UpdateUsers(IEnumerable<IDbUser> user)
        {
            foreach (var usr in user)
            {
                UpdateUser(usr);
            }
            return true;
        }

        public IEnumerable<IDbUser> GetAllUsers()
        {
            return _listUsers.Select(x => (IDbUser)x).ToList();
        }
    }
}
