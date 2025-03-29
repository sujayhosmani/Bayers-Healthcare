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
    public class ProviderQuery : IRequest<ValidatableResponse<HealthProviders>>
    {
        public string HealthProvidersId { get; set; }
    }

    public class ProviderQueryHandler : IRequestHandler<ProviderQuery, ValidatableResponse<HealthProviders>>
    {
        private readonly IDBContext _context;
        public ProviderQueryHandler(IDBContext context)
        {
            _context = context;
        }
        public async Task<ValidatableResponse<HealthProviders>> Handle(ProviderQuery request, CancellationToken cancellationToken)
        {
            HealthProviders HealthProviders = await _context.HealthProviders.Find(x => x.Id == request.HealthProvidersId).FirstOrDefaultAsync(cancellationToken);
            if (HealthProviders == null)
            {
                return new ValidatableResponse<HealthProviders>("HealthProviders not found", "HealthProviders not found", StatusCodes.Status404NotFound);
            }
            return new ValidatableResponse<HealthProviders>("success", null, HealthProviders, StatusCodes.Status200OK);
        }
    }
}
