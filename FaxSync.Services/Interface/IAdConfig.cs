using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services.Interface
{
    public interface IAdConfig
    {
        string TargetDomain { get; }
        string SearchRoot { get; }
        string Username { get; }
        string Password { get; }
        NameValueCollection GetConfigurationSettings { get; }
    }
}
