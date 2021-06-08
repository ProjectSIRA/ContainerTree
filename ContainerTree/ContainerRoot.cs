using Newtonsoft.Json;

namespace ContainerTree
{
    internal class ContainerRoot
    {
        [JsonProperty("version")]
        public string Version { get; set; } = null!;

        [JsonProperty("root")]
        public SerializedContainer Root { get; set; } = null!;
    }
}