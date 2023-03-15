using Newtonsoft.Json;

namespace Donations_App.Models.Payment
{
    public class response
    {
        [JsonProperty("success")]
        public bool success { get; set; }
    }
}
