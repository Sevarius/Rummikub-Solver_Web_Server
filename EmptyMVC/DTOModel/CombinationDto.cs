using System.Collections.Generic;
using Newtonsoft.Json;

namespace DTOModel
{
    [JsonObject]
    public class CombinationDto
    {
        [JsonProperty("cards")]
        public List<CardDto> Cards { get; set; }
    }
}