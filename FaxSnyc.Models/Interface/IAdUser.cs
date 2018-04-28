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
        string Office { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Role { get; set; }
        string JobTitle { get; set; }
        string CompanyName { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Country { get; set; }
        string ZipCode { get; set; }
        string PhoneNumber { get; set; }
        string MobileNumber { get; set; }
        string Language { get; set; }
    }
}
