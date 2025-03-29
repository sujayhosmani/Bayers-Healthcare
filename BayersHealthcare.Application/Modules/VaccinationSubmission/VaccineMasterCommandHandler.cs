using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using BayersHealthcare.Application.Modules.UserModule;
using BayersHealthcare.Common.ResponseInterceptor;
using BayersHealthcare.Domain;
using BayersHealthcare.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace BayersHealthcare.Application.Modules.VaccinationSubmission
{
    public class VaccinationSubmissionCommand : IRequest<ValidatableResponse<VaccinationSubmissions>>
    {
        public VaccinationSubmissions VaccineSubmission { get; set; }
    }

    public class VaccinationSubmissionCommandHandler : IRequestHandler<VaccinationSubmissionCommand, ValidatableResponse<VaccinationSubmissions>>
    {
        private readonly IDBContext _context;

        public VaccinationSubmissionCommandHandler(IDBContext context)
        {
            _context = context;
        }

        public async Task<ValidatableResponse<VaccinationSubmissions>> Handle(VaccinationSubmissionCommand request, CancellationToken cancellationToken)
        {
            if (request.VaccineSubmission == null)
            {
                return new ValidatableResponse<VaccinationSubmissions>("VaccineMaster object is null", "VaccineMaster object is null", StatusCodes.Status400BadRequest);
            }

            if (request.VaccineSubmission.Id == null)
            {
                await _context.VaccinationSubmissions.InsertOneAsync(request.VaccineSubmission, cancellationToken: cancellationToken);
               
                return new ValidatableResponse<VaccinationSubmissions>("Added succssfully", null, request.VaccineSubmission, StatusCodes.Status200OK);
            }
            else
            {
                FilterDefinition<VaccinationSubmissions> filter = Builders<VaccinationSubmissions>.Filter.Eq(p => p.Id, request.VaccineSubmission.Id);
                await _context.VaccinationSubmissions.ReplaceOneAsync(filter, request.VaccineSubmission , new ReplaceOptions { IsUpsert = true }, cancellationToken: cancellationToken);
                return new ValidatableResponse<VaccinationSubmissions>("Updated succssfully", null, request.VaccineSubmission, StatusCodes.Status200OK);
            }

        }
    }
}
