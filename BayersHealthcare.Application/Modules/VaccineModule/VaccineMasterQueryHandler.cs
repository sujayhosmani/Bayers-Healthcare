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

namespace BayersHealthcare.Application.Modules.VaccineModule
{
    public class VaccineMasterQuery : IRequest<ValidatableResponse<List<VaccinationMaster>>>
    {
    }

    public class VaccineMasterQueryHandler : IRequestHandler<VaccineMasterQuery, ValidatableResponse<List<VaccinationMaster>>>
    {
        private readonly IDBContext _context;
        public VaccineMasterQueryHandler(IDBContext context)
        {
            _context = context;
        }
        public async Task<ValidatableResponse<List<VaccinationMaster>>> Handle(VaccineMasterQuery request, CancellationToken cancellationToken)
        {
            List<VaccinationMaster> VaccinationMaster = await _context.VaccinationMaster.AsQueryable().ToListAsync();
            if (VaccinationMaster == null)
            {
                return new ValidatableResponse<List<VaccinationMaster>>("VaccinationMaster not found", "VaccinationMaster not found", StatusCodes.Status404NotFound);
            }
            return new ValidatableResponse<List<VaccinationMaster>>("success", null, VaccinationMaster, StatusCodes.Status200OK);
        }
    }
}
