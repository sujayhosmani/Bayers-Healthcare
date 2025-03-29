namespace BayersHealthcare.Common.ResponseInterceptor
{
    public class ResponseBody<TData> where TData : class
    {
        public string Message { get; }
        public TData? Data { get; }
        public IList<string>? Errors { get; }
        public Error? Error { get; set; }

        public ResponseBody(string message, IList<string>? errorMessages = null, TData? data = null)
        {
            Message = message;
            Errors = errorMessages;
            Data = data;
        }
    }
}