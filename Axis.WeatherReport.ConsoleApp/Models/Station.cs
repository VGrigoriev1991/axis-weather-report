using System.Text.Json.Serialization;

namespace Axis.WeatherReport.ConsoleApp.Models;

public record Station
{
    public string? Key { get; set; }

    public string? Name { get; set; }

    [JsonPropertyName("value")]
    public MeteorologyParameter[]? MeteorologyParameters { get; set; }
}
