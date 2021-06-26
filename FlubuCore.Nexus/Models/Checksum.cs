namespace FlubuCore.Nexus.Models
{
    using Newtonsoft.Json;

    public class Checksum
    {
        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }
    }
}