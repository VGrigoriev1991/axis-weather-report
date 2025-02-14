namespace Axis.WeatherReport.ConsoleApp.Models;

public record StationParameterDto
{
    public double Value { get; init; }

    public required string StationName { get; init; }
}
