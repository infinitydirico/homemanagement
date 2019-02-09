using Newtonsoft.Json;
using System.Collections.Generic;

namespace HomeManagement.AI.Vision.Entities
{
    public class VisionRecognitionResult
    {
        [JsonProperty("lines")]
        public List<Line> Lines { get; set; }
    }
}
