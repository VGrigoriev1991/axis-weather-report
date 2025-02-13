namespace Axis.WeatherReport.ConsoleApp.Models;

public record TotalRainfallDto : StationParameterDto
{
    public DateTime FromDate { get; init; }

    public DateTime ToDate { get; init; }
}
