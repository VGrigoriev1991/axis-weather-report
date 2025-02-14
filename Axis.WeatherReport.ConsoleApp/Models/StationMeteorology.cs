using System.Text.Json.Serialization;

namespace Axis.WeatherReport.ConsoleApp.Models;

public record StationMeteorology
{
    public Period? Period { get; set; }

    public Station? Station { get; set; }

    [JsonPropertyName("value")]
    public MeteorologyParameter[]? MeteorologyParameters { get; set; }
}
