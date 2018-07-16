using Newtonsoft.Json;

namespace Abominable_Intelligence.Model
{
    [JsonObject(Description = "Configuration data object")]
    public class ConfigData
    {
        [JsonProperty(PropertyName = "TrainingDataFolderName")]
        public string TrainingDataFolderName { get; set; }
        [JsonProperty(PropertyName = "SADataSetaName")]
        public string SADataSetaName { get; set; }
        [JsonProperty(PropertyName = "SAEvaluateDataSet")]
        public string SAEvaluateDataSet { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
