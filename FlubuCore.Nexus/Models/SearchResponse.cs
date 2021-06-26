namespace FlubuCore.Nexus.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class SearchResponse
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("continuationToken")]
        public object ContinuationToken { get; set; }
    }
}