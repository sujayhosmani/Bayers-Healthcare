using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BayersHealthcare.Common.Identity;
using BayersHealthcare.Common.ResponseInterceptor;
using BayersHealthcare.Domain;
using BayersHealthcare.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BayersHealthcare.Application.Modules.LoginModule
{
    public class LoginCommand : IRequest<ValidatableResponse<LoginResponse>>
    {
        public string phoneNumber { get; set; }
        public string password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ValidatableResponse<LoginResponse>>
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IDBContext _context;

        public LoginCommandHandler(IDBContext context, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ValidatableResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Users user = _context.Users.Find(x => x.PhoneNumber == request.phoneNumber && x.Password == request.password).FirstOrDefault();
            if (user == null)
            {
                return new ValidatableResponse<LoginResponse>("Invalid username or password", "invalid credentials", StatusCodes.Status400BadRequest);

            }

            LoginResponse response = _mapper.Map<LoginResponse>(user);
            GenerateTokenHandler generateToken = new(_config);
            response.Token = generateToken.GenerateToken(name: user.FirstName, role: user.Role, phone: user.PhoneNumber, id: user.Id);
            return new ValidatableResponse<LoginResponse>("success", null, response, StatusCodes.Status200OK);
        }
    }
}
