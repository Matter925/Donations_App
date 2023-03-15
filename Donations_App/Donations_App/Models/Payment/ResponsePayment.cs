using Newtonsoft.Json;

namespace Donations_App.Models.Payment
{
    public class ResponsePayment
    {
        [JsonProperty("obj")]
        public ResponseObj obj { get; set; }


    }
}
