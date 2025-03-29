using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using BayersHealthcare.Common.ResponseInterceptor;
using BayersHealthcare.Domain;
using BayersHealthcare.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace BayersHealthcare.Application.Modules.UserModule
{
    public class PatientQuery: IRequest<ValidatableResponse<Patient>>
    {
        public string PatientId { get; set; }
    }

    public class PatientQueryHandler : IRequestHandler<PatientQuery, ValidatableResponse<Patient>>
    {
        private readonly IDBContext _context;
        public PatientQueryHandler(IDBContext context)
        {
            _context = context;
        }
        public async Task<ValidatableResponse<Patient>> Handle(PatientQuery request, CancellationToken cancellationToken)
        {
            Patient patient = await _context.Patient.Find(x => x.Id == request.PatientId).FirstOrDefaultAsync(cancellationToken);
            if (patient == null)
            {
                return new ValidatableResponse<Patient>("Patient not found", "Patient not found", StatusCodes.Status404NotFound);
            }
            return new ValidatableResponse<Patient>("success", null, patient, StatusCodes.Status200OK);
        }
    }
}
