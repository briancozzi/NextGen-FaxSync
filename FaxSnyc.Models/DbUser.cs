using FaxSync.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Models
{
    public class DbUser : IDbUser
    {
        public string AttorneyUserID { get; set; }
        public string CurrentAssistantUserId { get; set; }
        public string CurrentFaxNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; } 
        public string PreviousAssistantUserId { get; set; }
        public string PreviousFaxNumber { get; set; }
    }
}
