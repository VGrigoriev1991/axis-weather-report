using Axis.WeatherReport.ConsoleApp.Models;

namespace Axis.WeatherReport.ConsoleApp.Services;

public interface IMeteorologyService
{
    Task<double> CalculateLatestHourAverageTemperatureAsync(CancellationToken cancellationToken);

    Task<TotalRainfallDto> CalculateTotalRainfallAsync(string stationId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<StationParameterDto>> GetInstantTemperaturePerStationAsync(CancellationToken cancellationToken);
}
