using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Domain
{
    public class User : IComparable<User>
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public bool Disabled { get; set; }
        public bool Excluded { get; set; }

        public int CompareTo(User other)
        {
            return string.Compare(this.UserId, other.UserId, true);
        }

    }
}
