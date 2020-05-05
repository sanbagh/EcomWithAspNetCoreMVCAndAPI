using System.Collections.Generic;

namespace API.Errors
{
    public class ApiValidationError : ApiResponse
    {
        public ApiValidationError() : base(400, null)
        {

        }
        public IList<string> Errors { get; set; }
    }
}