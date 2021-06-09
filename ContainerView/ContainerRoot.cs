using System.Text.Json.Serialization;

namespace ContainerView
{
    internal class ContainerRoot
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = null!;

        [JsonPropertyName("root")]
        public SerializedContainer Root { get; set; } = null!;
    }
}