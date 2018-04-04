using FaxSync.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services.Interface
{
    public interface IDbUserService: IUserService<IDbUser>
    {
        bool UpdateUser(IDbUser user);
        bool AddMissingUsers(IEnumerable<IAdUser> user);
        bool UpdateUsers(IEnumerable<IDbUser> user);
    }
}
