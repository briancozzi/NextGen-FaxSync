using FaxSync.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Models
{
    public class AdUser : IAdUser
    {
        public string AssistantDisplayName { get; set; }
        public string AssistantId { get; set; }
        public bool Disabled { get; set; }
        public string DisplayName { get; set; }
        public bool Excluded { get; set; }
        public string FaxNumber { get; set; }
        public string UserId { get; set; }

    }
}
