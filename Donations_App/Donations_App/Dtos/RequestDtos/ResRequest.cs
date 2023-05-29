using Donations_App.Models;

namespace Donations_App.Dtos.RequestDtos
{
    public class ResRequest
    {
        public bool Success { get; set; }
        public IEnumerable<Request> Requests { get; set; }

        public int Count { get; set; }
    }
}
