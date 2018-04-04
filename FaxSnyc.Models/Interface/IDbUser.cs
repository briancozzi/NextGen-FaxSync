using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Models.Interface
{
    public interface IDbUser
    {
        string AttorneyUserID { get; set; }
        string PreviousAssistantUserId { get; set; }
        string CurrentAssistantUserId { get; set; }
        string PreviousFaxNumber { get; set; }
        string CurrentFaxNumber { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
    }
}
