using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaxSync.Services.Interface;
using FaxSync.Models.Interface;
using FaxSync.Models;
using CHI.Context.ActiveDirectory;
using System.Configuration;
using FaxSync.Domain;

namespace FaxSync.Services
{
    public class AdUserService : IUserService<IAdUser>
    {
        UserServices _adUserService;
        public AdUserService()
        {
            var targetDomain = ConfigurationSettings.AppSettings["ManagedDomain"];
            var domainUser = ConfigurationSettings.AppSettings["DomainUser"];
            var userPassword = ConfigurationSettings.AppSettings["DomainPassword"];
            var searchRoot = ConfigurationSettings.AppSettings["SearchRoot"];
            var credenitals = GetAdCredentials(domainUser, userPassword, targetDomain);
            _adUserService = new UserServices(credenitals);
        }
        public AdUserService(IAdConfig config)
        {
            var credenitals = GetAdCredentials(config.Username, config.Password, config.TargetDomain);
            _adUserService = new UserServices(credenitals, config.SearchRoot);
        }
        public List<AdUser> LoadedUsers { get; private set; }
        public IEnumerable<IAdUser> GetAllUsers()
        {
            var users = _adUserService.GetAllUsers();

            return users.Select(x => new AdUser()
            {
                AssistantId = x.AssistantNetworkID,
                AssistantDisplayName = x.Assistant,
                Disabled = x.IsActive.Not(),
                DisplayName = x.DisplayName,
                Excluded = x.ExcludeFromFaxSyncScript.IsNotEmpty() && (x.ExcludeFromFaxSyncScript.CompareAreEqual("1") || x.ExcludeFromFaxSyncScript.CompareAreEqual("true")),
                FaxNumber = x.FaxNo,
                UserId = x.samAccountName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MobileNumber = x.MobileNo,
                PhoneNumber = x.Phone,
                State = x.State,
                Address = x.StreetAddress,
                City = x.City,
                Email = x.EmailAddress,
                JobTitle = x.JobTitle,
                Language = "en"            
                // Company["company"]
                // Language["preferredLanguage"]
                // Country["c"] 
                // ExternalId["objectGUID"]
                //[group_id: "l"]
            }).ToList();
            
            //LoadedUsers = new List<AdUser>();
            //var adUser = new AdUser();
            //adUser.UserId = "pkuchnicki";
            //adUser.DisplayName ="Paul Kuchnicki";
            //adUser.AssistantId = "pkuchnicki";
            //adUser.Disabled = false;
            //adUser.FaxNumber = "+14123942555";
            //adUser.Excluded = false;
            //LoadedUsers.Add(adUser);
            //return LoadedUsers;
        }

        public IEnumerable<IAdUser> GetCachedUsers(bool refreshCache = false)
        {
            if (LoadedUsers == null || refreshCache)
                LoadedUsers = GetAllUsers().Select(x=> (AdUser)x).ToList();

            return LoadedUsers;
        }

        public IAdUser GetUserById(string userId,bool refreshCache = false)
        {
            LoadedUsers = GetCachedUsers(refreshCache).Select(x => (AdUser)x).ToList();
            return LoadedUsers.Where(x=> string.Compare(x.UserId, userId,true) == 0).FirstOrDefault();
        }
        private ConnectionCredentials GetAdCredentials(string userName,string password, string targetDomain)
        {
            ConnectionCredentials creds = new ConnectionCredentials(targetDomain);
            try
            {
                string domainUser = userName;
                string userPassword = password;
                if (!string.IsNullOrEmpty(domainUser))
                {
                    if (domainUser.Contains(@"\"))
                    {
                        string[] parts = domainUser.Split('\\');

                        creds.UserDomain = parts[0];
                        domainUser = parts[1];
                    }
                    creds.UserName = domainUser;
                    creds.Password = userPassword;
                    if (userPassword.Length > 15)
                    {
                        // creds.Password = Dpapi.Decrypt(userPassword, _IV);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return creds;
        }
    }
}
