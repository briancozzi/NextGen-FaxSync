using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Models.Interface
{
    public interface IAdUser
    {
        string UserId { get; set; }
        string DisplayName { get; set; }
        string FaxNumber { get; set; }
        string AssistantId { get; set; } 
        string AssistantDisplayName { get; set; }
        bool Disabled { get; set; }
        bool Excluded { get; set; }
    }
}
