using BayersHealthcare.Application.Modules.UserModule;
using BayersHealthcare.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bayers_Healthcare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("patient/{id}")]
        public async Task<IActionResult> Patient(string id)
        {
            var response = await _mediator.Send(new PatientQuery() { PatientId = id });
            return response.ResponseData;
        }

        [HttpGet("provider/{id}")]
        public async Task<IActionResult> HealthProvider(string id)
        {
            var response = await _mediator.Send(new ProviderQuery() { HealthProvidersId = id });
            return response.ResponseData;
        }

        
    }
}
