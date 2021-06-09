using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContainerView
{
    internal class SerializedContainer
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;

        [JsonPropertyName("installers")]
        public string[] Installers { get; set; } = Array.Empty<string>();

        [JsonPropertyName("contracts")]
        public string[] Contracts { get; set; } = Array.Empty<string>();

        [JsonPropertyName("children")]
        public List<SerializedContainer> Children { get; set; } = new();
    }
}