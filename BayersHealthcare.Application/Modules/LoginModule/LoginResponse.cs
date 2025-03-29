using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BayersHealthcare.Domain;

namespace BayersHealthcare.Application.Modules.LoginModule
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string? FirstName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
