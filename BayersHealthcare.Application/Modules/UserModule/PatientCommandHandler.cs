using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BayersHealthcare.Common.ResponseInterceptor;
using BayersHealthcare.Domain;
using BayersHealthcare.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BayersHealthcare.Application.Modules.UserModule
{
    public class PatientCommand : IRequest<ValidatableResponse<Patient>>
    {
        public Patient Patient { get; set; }
    }
    public class PatientCommandHandler: IRequestHandler<PatientCommand, ValidatableResponse<Patient>>
    {
        private readonly IDBContext _context;

        public PatientCommandHandler(IDBContext context)
        {
            _context = context;
        }

        public async Task<ValidatableResponse<Patient>> Handle(PatientCommand request, CancellationToken cancellationToken)
        {
            if(request.Patient == null)
            {
                return new ValidatableResponse<Patient>("Patient object is null", "Patient object is null", StatusCodes.Status400BadRequest);
            }

            if (request.Patient.Id == null)
            {
                await _context.Patient.InsertOneAsync(request.Patient, cancellationToken: cancellationToken);
                Users users = new Users
                {
                    FirstName = request.Patient.FirstName,
                    PhoneNumber = request.Patient.PhoneNumber,
                    Role = "Patient",
                    Password = request.Patient.Password,
                    Age = request.Patient.Age,
                    CreatedBy = request.Patient.CreatedBy,
                    CreatedDateTime = DateTime.Now,
                    UpdatedBy = request.Patient.UpdatedBy,
                    UpdatedDateTime = DateTime.Now,
                    Dob = request.Patient.Dob,
                    Gender = request.Patient.Gender,
                    UserId = request.Patient.Id
                };
                await _context.Users.InsertOneAsync(users, cancellationToken: cancellationToken);
                return new ValidatableResponse<Patient>("Added succssfully", null, request.Patient, StatusCodes.Status200OK);
            }
            else
            {
                FilterDefinition<Patient> filter = Builders<Patient>.Filter.Eq(p => p.Id, request.Patient.Id);
                await _context.Patient.ReplaceOneAsync(filter, request.Patient, new ReplaceOptions { IsUpsert = true }, cancellationToken: cancellationToken);
                return new ValidatableResponse<Patient>("Updated succssfully", null, request.Patient, StatusCodes.Status200OK);
            }
            
        }
    }
}
