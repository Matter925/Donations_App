using Newtonsoft.Json;

namespace Donations_App.Models.Payment
{
    public class ResponseOrder
    {
        [JsonProperty("id")]
        public int id { get; set; }
    }
}
