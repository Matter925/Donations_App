using Newtonsoft.Json;

namespace Donations_App.Models.Payment
{
    public class ResponseObj
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("amount_cents")]
        public double amount_cents { get; set; }

        [JsonProperty("success")]
        public bool success { get; set; }

        [JsonProperty("order")]
        public ResponseOrder order { get; set; }

    }
}
