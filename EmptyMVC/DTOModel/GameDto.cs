using System.Collections.Generic;
using Newtonsoft.Json;

namespace DTOModel
{
    [JsonObject]
    public class GameDto
    {
        [JsonProperty("table")]
        public List<CombinationDto> Table { get; set; }

        [JsonProperty("hand")]
        public List<CardDto> Hand { get; set; }
    }
}