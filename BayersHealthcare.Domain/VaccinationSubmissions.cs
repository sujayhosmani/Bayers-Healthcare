using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayersHealthcare.Domain
{
    public class VaccinationSubmissions: VaccinationMaster
    {
        public string? VaccineId { get; set; }
        public string? HealthProviderId { get; set; }
        public string? PatientId { get; set; }
        public DateTime VaccinationDate { get; set; }
        public bool IsVaccinated { get; set; }
        public string? Status { get; set; }
    }
}
