using Newtonsoft.Json;
using System.Collections.Generic;

namespace HomeManagement.AI.Vision.Entities
{
    public class Line : Box
    {
        [JsonProperty("words")]
        public List<Box> Words { get; set; }
    }
}
