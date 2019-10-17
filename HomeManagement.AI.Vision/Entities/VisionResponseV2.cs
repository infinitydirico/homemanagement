using Newtonsoft.Json;
using System.Collections.Generic;

namespace HomeManagement.AI.Vision.Entities
{
    public class VisionResponseV2
    {
        [JsonProperty("regions")]
        public List<VisionRecognitionResult> RecognitionResult { get; set; }
    }
}
