using FaxSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services.Interface
{
    public interface IFaxApiService
    {
        ApiResult UnAssignUser(int faxNumberId, int faxUserId);
        ApiResult AssignUser(int faxNumberId, int faxUserId);
    }
}
