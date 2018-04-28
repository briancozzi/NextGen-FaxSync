using FaxSync.Models.FaxApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSnyc.Models.Sync
{
    public class FaxApiUserActionSync : FaxApiUser
    {
        public string password_setup_type { get; set; }
        public string password { get; set; }
        public string password_confirmation { get; set; }
    }

    


}
