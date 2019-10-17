using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.AI.Vision.Entities
{
    public class Line
    {
        [JsonProperty("words")]
        public List<Box> Words { get; set; }

        public string Text
        {
            get
            {
                return string.Join(" ", Words.Select(x => x.Text));
            }
        }
    }
}
