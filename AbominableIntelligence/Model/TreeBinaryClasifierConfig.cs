using Newtonsoft.Json;

namespace Abominable_Intelligence.Model
{
    public class TreeBinaryClasifierConfig
    {
        [JsonProperty(PropertyName = "NumLeaves")]
        public int NumLeaves { get; set; }
        [JsonProperty(PropertyName = "NumTrees")]
        public int NumTrees { get; set; }
        [JsonProperty(PropertyName = "MinDocumentsInLeafs")]
        public int MinDocumentsInLeafs { get; set; }
    }
}
