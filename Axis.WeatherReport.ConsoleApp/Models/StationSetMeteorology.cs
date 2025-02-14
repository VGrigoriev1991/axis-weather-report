using System.Text.Json.Serialization;

namespace Axis.WeatherReport.ConsoleApp.Models;

public record StationSetMeteorology
{
    [JsonPropertyName("station")]
    public Station[]? Stations { get; set; }
}
