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
            AutoSyncTest();
        }
        static void AutoSyncTest()
        {
            var syncService = new SyncService();
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

            var resultAssign = faxApi.AddUser(paul.main_fax_number_id, paul.id);
            var resultUnassigm = faxApi.RemoveUser(paul.main_fax_number_id, paul.id);
            resultAssign = faxApi.AddUser(paul.main_fax_number_id, paul.id);
        }
    }
}
