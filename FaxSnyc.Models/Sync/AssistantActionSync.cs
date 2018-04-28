using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSnyc.Models.Sync
{
    public class AssistantActionSync
    {
        public string AttorneyId { get; set; }
        public string UserId { get; set; }
        public string FaxNumber { get; set; }
        public int FaxAttorneyUserId { get; set; }
        public int FaxUserId { get; set; }
        public int FaxNumberId { get; set; }
        public bool FaxNumberIsShared { get; set; }
    }
}
