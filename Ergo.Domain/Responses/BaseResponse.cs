namespace Ergo.Application.Responses
{
    public class BaseResponse
    {
        public BaseResponse() => Success = true;

        public BaseResponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }

        public List<string>? ValidationsErrors { get; set; }
    }
}
