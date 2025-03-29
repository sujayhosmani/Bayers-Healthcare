using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayersHealthcare.Domain
{
    public class VaccinationMaster: AuditableEntity
    {
        public string VaccineName { get; set; }

    }
}
