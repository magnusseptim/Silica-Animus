using Newtonsoft.Json;

namespace Silica_Animus.Model
{
    public class IdentityConf
    {
        [JsonProperty(PropertyName = "DBConnString")]
        public string DBConnString { get; set; }
        [JsonProperty(PropertyName = "JwtKey")]
        public string JwtKey { get; set; }
        [JsonProperty(PropertyName = "JwtIssuer")]
        public string JwtIssuer { get; set; }
        [JsonProperty(PropertyName = "JwtExpireDays")]
        public string JwtExpireDays { get; set; }
    }
}
