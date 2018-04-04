using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Domain
{
    public class FaxNumber : IComparable<FaxNumber>
    {
        public string Number { get; set; }
        public bool IsBlacklisted { get; set; }

        public FaxNumber(string number) : this(number, null)
        { }
        public FaxNumber(string number, List<string> blackListedFaxNumbers)
        {
            this.Number = number;
            this.SetIfBlacklisted(blackListedFaxNumbers);
        }

        public int CompareTo(FaxNumber other)
        {
            return string.Compare(this.Number, other.Number, true);
        }
        public void SetIfBlacklisted(List<string> blackListedFaxNumbers)
        {
            if (blackListedFaxNumbers == null) return;

            IsBlacklisted = blackListedFaxNumbers.Contains(Number);
        }
    }
}
