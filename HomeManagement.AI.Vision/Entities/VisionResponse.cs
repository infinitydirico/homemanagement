using Newtonsoft.Json;
using System.Collections.Generic;

namespace HomeManagement.AI.Vision.Entities
{
    public class VisionResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("finished")]
        public bool Finished { get; set; }

        [JsonProperty("recognitionResults")]
        public List<VisionRecognitionResult> RecognitionResult { get; set; }
    }
}
