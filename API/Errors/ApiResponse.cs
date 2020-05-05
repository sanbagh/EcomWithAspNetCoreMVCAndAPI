namespace API.Errors
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public ApiResponse(int code, string message = null)
        {
            Code = code;
            Message = message ?? GetDefaultMessageForErrorCode(code);
        }
        private string GetDefaultMessageForErrorCode(int code)
        {
            return code switch
            {
                400 => "A bad request you have made",
                401 => "You are not authorized",
                404 => "Resource you are looking for, Not Found",
                500 => "Some error occured at server",
                _ => null
            };
        }
    }
}