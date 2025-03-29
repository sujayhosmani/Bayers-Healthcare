using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BayersHealthcare.Common.ResponseInterceptor;
using BayersHealthcare.Domain;
using BayersHealthcare.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace BayersHealthcare.Application.Modules.UserModule
{
    public class ProviderCommand : IRequest<ValidatableResponse<HealthProviders>>
    {
        public HealthProviders HealthProviders { get; set; }
    }
    public class ProviderCommandHandler : IRequestHandler<ProviderCommand, ValidatableResponse<HealthProviders>>
    {
        private readonly IDBContext _context;

        public ProviderCommandHandler(IDBContext context)
        {
            _context = context;
        }

        public async Task<ValidatableResponse<HealthProviders>> Handle(ProviderCommand request, CancellationToken cancellationToken)
        {
            if (request.HealthProviders == null)
            {
                return new ValidatableResponse<HealthProviders>("HealthProviders object is null", "HealthProviders object is null", StatusCodes.Status400BadRequest);
            }

            if (request.HealthProviders.Id == null)
            {
                await _context.HealthProviders.InsertOneAsync(request.HealthProviders, cancellationToken: cancellationToken);
                Users users = new Users
                {
                    FirstName = request.HealthProviders.FirstName,
                    PhoneNumber = request.HealthProviders.PhoneNumber,
                    Role = "HealthProviders",
                    Password = request.HealthProviders.Password,
                    Age = request.HealthProviders.Age,
                    CreatedBy = request.HealthProviders.CreatedBy,
                    CreatedDateTime = DateTime.Now,
                    UpdatedBy = request.HealthProviders.UpdatedBy,
                    UpdatedDateTime = DateTime.Now,
                    Dob = request.HealthProviders.Dob,
                    Gender = request.HealthProviders.Gender,
                    UserId = request.HealthProviders.Id
                };
                await _context.Users.InsertOneAsync(users, cancellationToken: cancellationToken);
                return new ValidatableResponse<HealthProviders>("Added succssfully", null, request.HealthProviders, StatusCodes.Status200OK);
            }
            else
            {
                FilterDefinition<HealthProviders> filter = Builders<HealthProviders>.Filter.Eq(p => p.Id, request.HealthProviders.Id);
                await _context.HealthProviders.ReplaceOneAsync(filter, request.HealthProviders, new ReplaceOptions { IsUpsert = true }, cancellationToken: cancellationToken);
                return new ValidatableResponse<HealthProviders>("Updated succssfully", null, request.HealthProviders, StatusCodes.Status200OK);
            }

        }
    }
}
