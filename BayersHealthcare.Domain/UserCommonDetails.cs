using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BayersHealthcare.Domain
{
    public class UserCommonDetails: AuditableEntity
    {
        public string? FirstName { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public bool IsConsentChecked { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public string? Password { get; set; }
    }
}
