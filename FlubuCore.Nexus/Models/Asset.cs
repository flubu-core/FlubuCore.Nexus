namespace FlubuCore.Nexus.Models
{
    using Newtonsoft.Json;

    public class Asset
    {
        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("repository")]
        public string Repository { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("checksum")]
        public Checksum Checksum { get; set; }
    }
}