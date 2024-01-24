using System.Net;

namespace MagicVilla_Coupon.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages= new List<string>();
        }
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
        public HttpStatusCode  StatusCode { get; set; }
        public List<String> ErrorMessages { get; set; }
    }
}
