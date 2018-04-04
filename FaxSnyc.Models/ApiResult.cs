using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Models
{
    public class ApiResult
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string ApiCall { get; set; }
        public string HttpCallLog { get; set; }
    }
}
