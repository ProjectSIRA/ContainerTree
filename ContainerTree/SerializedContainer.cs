using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ContainerTree
{
    internal class SerializedContainer
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
        
        [JsonProperty("address")]
        public string Address { get; set; } = null!;

        [JsonProperty("installers")]
        public string[] Installers { get; set; } = Array.Empty<string>();

        [JsonProperty("contracts")]
        public string[] Contracts { get; set; } = Array.Empty<string>();

        [JsonProperty("children")]
        public List<SerializedContainer> Children { get; set; } = new();
    }
}