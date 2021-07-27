using Newtonsoft.Json;

namespace DTOModel
{
    [JsonObject]
    public class CardDto
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }
    }
}