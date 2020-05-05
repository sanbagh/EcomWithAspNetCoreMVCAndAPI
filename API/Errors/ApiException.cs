namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int code, string message = null, string details = null) : base(code, message)
        {
            Details = details;
        }

        public string Details { get; }
    }
}