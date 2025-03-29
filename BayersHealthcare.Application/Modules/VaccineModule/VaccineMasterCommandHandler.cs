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

namespace BayersHealthcare.Application.Modules.VaccineModule
{
    public class VaccineMasterCommand : IRequest<ValidatableResponse<VaccinationMaster>>
    {
        public VaccinationMaster VaccineMaster { get; set; }
    }

    public class VaccineMasterCommandHandler : IRequestHandler<VaccineMasterCommand, ValidatableResponse<VaccinationMaster>>
    {
        private readonly IDBContext _context;

        public VaccineMasterCommandHandler(IDBContext context)
        {
            _context = context;
        }

        public async Task<ValidatableResponse<VaccinationMaster>> Handle(VaccineMasterCommand request, CancellationToken cancellationToken)
        {
            if (request.VaccineMaster == null)
            {
                return new ValidatableResponse<VaccinationMaster>("VaccineMaster object is null", "VaccineMaster object is null", StatusCodes.Status400BadRequest);
            }

            if (request.VaccineMaster.Id == null)
            {
                await _context.VaccinationMaster.InsertOneAsync(request.VaccineMaster, cancellationToken: cancellationToken);
               
                return new ValidatableResponse<VaccinationMaster>("Added succssfully", null, request.VaccineMaster, StatusCodes.Status200OK);
            }
            else
            {
                FilterDefinition<VaccinationMaster> filter = Builders<VaccinationMaster>.Filter.Eq(p => p.Id, request.VaccineMaster.Id);
                await _context.VaccinationMaster.ReplaceOneAsync(filter, request.VaccineMaster , new ReplaceOptions { IsUpsert = true }, cancellationToken: cancellationToken);
                return new ValidatableResponse<VaccinationMaster>("Updated succssfully", null, request.VaccineMaster, StatusCodes.Status200OK);
            }

        }
    }
}
