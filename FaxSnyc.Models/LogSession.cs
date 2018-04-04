using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSnyc.Models
{
    public class LogSession
    {
        public int SessionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}
