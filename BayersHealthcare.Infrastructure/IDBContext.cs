using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BayersHealthcare.Domain;
using MongoDB.Driver;

namespace BayersHealthcare.Infrastructure
{
    public interface IDBContext
    {
        IMongoCollection<HealthProviders> HealthProviders { get; set; }
        IMongoCollection<Patient> Patient { get; set; }
        IMongoCollection<Users> Users { get; set; }
        IMongoCollection<VaccinationMaster> VaccinationMaster { get; set; }
        IMongoCollection<VaccinationSubmissions> VaccinationSubmissions { get; set; }
    }
}
