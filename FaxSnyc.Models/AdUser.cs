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
        public string Office { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }

    }
}
