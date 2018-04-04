using FaxSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaySync.Models.Interface
{
    public interface ILogResults
    {
        int SessionId { get; set; }
        string UserId { get; set; }
        string FieldName { get; set; }
        string PreviousValue { get; set; }
        string NewValue { get; set; }
        bool Result { get; set; }
        string Message  { get; set; }
        LogType Type { get; set; }

    }
}
