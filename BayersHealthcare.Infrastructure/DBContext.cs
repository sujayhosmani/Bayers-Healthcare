
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using BayersHealthcare.Domain;


namespace BayersHealthcare.Infrastructure
{
    public class DBContext : IDBContext
    {

        public DBContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(configuration.GetConnectionString("DefaultConnection"));

            settings.LinqProvider = LinqProvider.V3;

            var client = new MongoClient(settings);
            string name = configuration["DatabaseSettings:DatabaseName"];
            var database = client.GetDatabase(name);
            //TODO Add unique keys index savechanges

            
            Patient = database.GetCollection<Patient>(nameof(Patient));
            HealthProviders = database.GetCollection<HealthProviders>(nameof(HealthProviders));
            Users = database.GetCollection<Users>(nameof(Users));
            VaccinationMaster = database.GetCollection<VaccinationMaster>(nameof(VaccinationMaster));
            VaccinationSubmissions = database.GetCollection<VaccinationSubmissions>(nameof(VaccinationSubmissions));

            CreateUsersIndex(Users);
            CreateVaccineIndex(VaccinationMaster);
        }

        private async static void CreateUsersIndex(IMongoCollection<Users> users)
        {
            var name = new IndexKeysDefinitionBuilder<Users>().Ascending(e => e.FirstName);

            var uniqueTrue = new CreateIndexOptions() { Unique = true };
            var uniqueFalse = new CreateIndexOptions() { Unique = false };
            List<CreateIndexModel<Users>> indexModel = new()
            {
                new(name, uniqueFalse),
            };
            await users.Indexes.CreateManyAsync(indexModel);
        }

        private async static void CreateVaccineIndex(IMongoCollection<VaccinationMaster> vacc)
        {
            var name = new IndexKeysDefinitionBuilder<VaccinationMaster>().Ascending(e => e.VaccineName);

            var uniqueTrue = new CreateIndexOptions() { Unique = true };
            var uniqueFalse = new CreateIndexOptions() { Unique = false };
            List<CreateIndexModel<VaccinationMaster>> indexModel = new()
            {
                new(name, uniqueTrue),
            };
            await vacc.Indexes.CreateManyAsync(indexModel);
        }


        public IMongoCollection<HealthProviders> HealthProviders { get; set; }
        public IMongoCollection<Patient> Patient { get; set; }
        public IMongoCollection<Users> Users { get; set; }
        public IMongoCollection<VaccinationMaster> VaccinationMaster { get; set; }
        public IMongoCollection<VaccinationSubmissions> VaccinationSubmissions { get; set; }


    }
}

