using AutoMapper;
using BayersHealthcare.Application.Modules.LoginModule;
using BayersHealthcare.Domain;

namespace BayersHealthcare.Application
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Users, LoginResponse>().ReverseMap();
             
            });

            return mappingConfig;
        }
    }
}