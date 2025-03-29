using BayersHealthcare.Application.Modules.LoginModule;
using BayersHealthcare.Application.Modules.UserModule;
using BayersHealthcare.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bayers_Healthcare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommand loginCommand)
        {
            var response = await _mediator.Send(loginCommand);
            return response.ResponseData;
        }

        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] Patient patient)
        {
            var response = await _mediator.Send(new PatientCommand() { Patient = patient });
            return response.ResponseData;
        }

        [HttpPost("register/provider")]
        public async Task<IActionResult> RegisterProvider([FromBody] HealthProviders provider)
        {
            var response = await _mediator.Send(new ProviderCommand() { HealthProviders = provider });
            return response.ResponseData;
        }
    }
}
