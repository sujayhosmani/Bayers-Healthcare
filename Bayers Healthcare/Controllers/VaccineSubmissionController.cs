using BayersHealthcare.Application.Modules.VaccinationSubmission;
using BayersHealthcare.Application.Modules.VaccineModule;
using BayersHealthcare.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bayers_Healthcare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineSubmissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VaccineSubmissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("vaccine/submission")]
        public async Task<IActionResult> VaccineSubmission()
        {
            var response = await _mediator.Send(new VaccinationSubmissionQuery());
            return response.ResponseData;
        }

        [HttpPost("vaccine/submission")]
        public async Task<IActionResult> VaccineSubmission(VaccinationSubmissions vs)
        {
            var response = await _mediator.Send(new VaccinationSubmissionCommand() { VaccineSubmission = vs });
            return response.ResponseData;
        }
    }
}
