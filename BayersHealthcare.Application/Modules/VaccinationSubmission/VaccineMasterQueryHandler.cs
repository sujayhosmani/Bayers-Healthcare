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

namespace BayersHealthcare.Application.Modules.VaccinationSubmission
{
    public class VaccinationSubmissionQuery : IRequest<ValidatableResponse<List<VaccinationSubmissions>>>
    {
    }

    public class VaccinationSubmissionQueryHandler : IRequestHandler<VaccinationSubmissionQuery, ValidatableResponse<List<VaccinationSubmissions>>>
    {
        private readonly IDBContext _context;
        public VaccinationSubmissionQueryHandler(IDBContext context)
        {
            _context = context;
        }
        public async Task<ValidatableResponse<List<VaccinationSubmissions>>> Handle(VaccinationSubmissionQuery request, CancellationToken cancellationToken)
        {
            List<VaccinationSubmissions> VaccinationSubmissions = await _context.VaccinationSubmissions.AsQueryable().ToListAsync();
            if (VaccinationSubmissions == null)
            {
                return new ValidatableResponse<List<VaccinationSubmissions>>("VaccinationSubmissions not found", "VaccinationSubmissions not found", StatusCodes.Status404NotFound);
            }
            return new ValidatableResponse<List<VaccinationSubmissions>>("success", null, VaccinationSubmissions, StatusCodes.Status200OK);
        }
    }
}
