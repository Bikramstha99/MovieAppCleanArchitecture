using System.Net;

namespace MovieAppAPI.ViewModel
{
    public class Responses<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public int HttpStatus { get; set; }
        public T Data { get; set; }
        public string ReceiveEmail { get; set; }
    }
}
