using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayersHealthcare.Common.ResponseInterceptor
{
    public class ValidatableResponse<TModel> : ControllerBase
        where TModel : class
    {
        private readonly IList<string>? _errorMessages;
        public bool Success => StatusCode < 400 ? true : false;
        public new int StatusCode { get; set; }
        public string? Message  { get; set; }
        public ResponseBody<TModel> ResponseBody { get; }

        public ValidatableResponse(string message, IList<string>? errorMessage = null, TModel? data = null, int statusCode = 500)
        {
            _errorMessages = errorMessage ?? new List<string>();

            if (StatusCode == 0)
            {
                StatusCode = statusCode;
            }

            if (_errorMessages.Count > 0)
            {
                ResponseBody = new ResponseBody<TModel>(message, errorMessage, data);
                StatusCode = 400;
                Message = "Please Correct the following errors " + string.Join(",", _errorMessages);
            }
            else
            {
                ResponseBody = new ResponseBody<TModel>(message, errorMessage, data);
            }
        }

        public ValidatableResponse(string message, string errorMessage, int statusCode)
        {
            ResponseBody = new ResponseBody<TModel>(message, new List<string>() { errorMessage });
            StatusCode = statusCode;
            Message = errorMessage;
            ResponseBody.Error = new Error() { Code = StatusCode, Description = message, Reason = errorMessage };

        }

        public ValidatableResponse()
        {
        }

        public ActionResult ResponseData
        {
            get
            {
                switch (StatusCode)
                {
                    case 400:
                    case 401:
                    case 404:
                    case 500:
                    case 403:
                        return new ObjectResult(ResponseBody) { StatusCode = StatusCode };
                    case 200:
                        return Ok(ResponseBody.Data);
                    default:
                        return BadRequest(null);
                }
            }
        }

    }
}
