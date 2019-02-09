using Newtonsoft.Json;

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

        [JsonProperty("recognitionResult")]
        public VisionRecognitionResult RecognitionResult { get; set; }
    }
}
