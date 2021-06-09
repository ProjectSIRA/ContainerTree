using System.Text.Json.Serialization;

namespace ContainerView
{
    public class GitHubContent
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}