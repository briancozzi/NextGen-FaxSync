using FaxSync.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services
{
    public class SyncUserService : SyncService, IFaxSyncService
    {
        public void Sync(int? logSessionId = null)
        {
            throw new NotImplementedException();
        }
    }
}
