using FaySync.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Models
{
    public class LogResult : ILogResults
    {
        public string FieldName { get; set; }   
        public string Message { get; set; }
        public string NewValue { get; set; }
        public string PreviousValue { get; set; }
        public bool Result { get; set; }
        public int SessionId { get; set; }
        public LogType Type { get; set; }
        public string UserId { get; set; }


    }
}
