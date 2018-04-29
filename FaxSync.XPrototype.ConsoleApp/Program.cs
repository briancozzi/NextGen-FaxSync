using FaxSync.Domain;
using FaxSync.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Prototype.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoSyncUsersTest();
        }
        static void AutoSyncAssitantsTest()
        {
            var syncService = new SyncAssistantService();
            syncService.Sync();
            Console.Read();
        }
        static void AutoSyncUsersTest()
        {
            var syncService = new SyncUserService();
            syncService.Sync();
            Console.Read();
        }
        static void ManualTest()
        {
            var faxApi = new FaxApiService();
            var users = faxApi.GetAllUsers();
            var numbers = faxApi.GetAllFaxNumbers();
            var paulUserName = "pkuchnicki";
            var paul = users.data.Where(x => x.username.CompareAreEqual(paulUserName))
                                 .FirstOrDefault();

            var resultAssign = faxApi.AssignUser(paul.main_fax_number_id, paul.id);
            var resultUnassigm = faxApi.UnAssignUser(paul.main_fax_number_id, paul.id);
            resultAssign = faxApi.AssignUser(paul.main_fax_number_id, paul.id);
        }
    }
}
