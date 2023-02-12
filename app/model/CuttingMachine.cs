using System.Text.Json.Serialization;

namespace lantek.model;


public class CuttingMachine{

    [JsonPropertyName("id")]
    public Guid id { get; set; }

    [JsonPropertyName("name")]
    public string? name { get; set; }

    [JsonPropertyName("manufacturer")]
    public string? manufacturer { get; set; }
}