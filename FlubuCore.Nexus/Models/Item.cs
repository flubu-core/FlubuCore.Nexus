namespace FlubuCore.Nexus.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("repository")]
        public string Repository { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public Version Version { get; set; }

        [JsonProperty("assets")]
        public List<Asset> Assets { get; set; }
    }
}