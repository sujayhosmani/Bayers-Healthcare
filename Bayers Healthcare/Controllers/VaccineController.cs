using BayersHealthcare.Application.Modules.UserModule;
using BayersHealthcare.Application.Modules.VaccineModule;
using BayersHealthcare.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bayers_Healthcare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VaccineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("vaccine")]
        public async Task<IActionResult> Vaccine()
        {
            var response = await _mediator.Send(new VaccineMasterQuery());
            return response.ResponseData;
        }

        [HttpPost("vaccine")]
        public async Task<IActionResult> Vaccine(VaccinationMaster vaccinationMaster)
        {
            var response = await _mediator.Send(new VaccineMasterCommand() { VaccineMaster = vaccinationMaster });
            return response.ResponseData;
        }
    }
}
