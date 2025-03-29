using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayersHealthcare.Domain
{
    public class Patient: UserCommonDetails
    {
        public string PastHistory { get; set; }
        public string Allergies { get; set; }

    }
}
