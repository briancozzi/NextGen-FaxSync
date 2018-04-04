using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services.Interface
{
    public interface IUserService<T> where T: class
    {
         IEnumerable<T> GetAllUsers();
    }
}
