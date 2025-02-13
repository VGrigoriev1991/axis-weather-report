using System.Globalization;
using Axis.WeatherReport.ConsoleApp.Clients;
using Axis.WeatherReport.ConsoleApp.Models;

namespace Axis.WeatherReport.ConsoleApp.Services;

/// <summary>
/// Service provides various meteorology-related calculations and data aggregation.
/// </summary>
/// <param name="stationSetMeteorologyClient"></param>
/// <param name="stationMeteorologyClient"></param>
public class MeteorologyService(
    IStationSetMeteorologyClient stationSetMeteorologyClient,
    IStationMeteorologyClient stationMeteorologyClient)
    : IMeteorologyService
{
    // Placeholder for undefined station names in the response.
    private const string UndefinedStationName = "undefined";

    /// <summary>
    /// Calculates the average temperature for the latest hour across all available stations.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<double> CalculateLatestHourAverageTemperatureAsync(CancellationToken cancellationToken)
    {
        // Fetches temperature data for all stations for the last hour
        var clientResponse = await stationSetMeteorologyClient.GetAsync(
            MeteorologyParameterKey.InstantTemperaturePerHour,
            StationSetKey.All,
            PeriodParameterKey.LatestHour,
            cancellationToken);

        // Ensure successful HTTP response.
        // Note: It is possible to implement a separate service for processing responses from a specific provider client.
        // This service can generate and throw the necessary types of exceptions,
        // which will then be caught at the main level (Middleware) before being displayed to the user.
        // At this time, we do not expect any errors that need to be handled in a special way.
        await clientResponse.EnsureSuccessStatusCodeAsync();

        // Process the response and calculate average temperature
        var availableTemperatures = clientResponse.Content?.Stations?
                                        .Where(x => x.MeteorologyParameters?.Length > 0)
                                        .Select(x => x.MeteorologyParameters!.First().Value)
                                        .Where(
                                            x => !string.IsNullOrWhiteSpace(x) && double.TryParse(x, CultureInfo.InvariantCulture, out _))
                                        .Select(x => double.Parse(x!, CultureInfo.InvariantCulture))
                                        .ToArray()
                                    ?? [];

        return availableTemperatures.Length > 0 ? availableTemperatures.Sum() / availableTemperatures.Length : default;
    }

    /// <summary>
    /// Calculates the total rainfall for a specific station for recent months.
    /// </summary>
    /// <param name="stationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TotalRainfallDto> CalculateTotalRainfallAsync(string stationId, CancellationToken cancellationToken)
    {
        // Fetches rainfall data for specific station over the latest months
        var clientResponse = await stationMeteorologyClient.GetAsync(
            MeteorologyParameterKey.TotalRainfallPerDay,
            stationId,
            PeriodParameterKey.LatestMonths,
            cancellationToken);

        // Ensure successful HTTP response.
        // Note: It is possible to implement a separate service for processing responses from a specific provider client.
        // This service can generate and throw the necessary types of exceptions,
        // which will then be caught at the main level (Middleware) before being displayed to the user.
        // At this time, we do not expect any errors that need to be handled in a special way.
        await clientResponse.EnsureSuccessStatusCodeAsync();

        // Calculate the total rainfall utilizing the response data
        var totalRainfall = clientResponse.Content?.MeteorologyParameters?.Select(x => x.Value)
                                .Where(x => !string.IsNullOrWhiteSpace(x) && double.TryParse(x, CultureInfo.InvariantCulture, out _))
                                .Select(x => double.Parse(x!, CultureInfo.InvariantCulture))
                                .Sum()
                            ?? default;

        return new TotalRainfallDto
        {
            Value = totalRainfall,
            StationName = clientResponse.Content?.Station?.Name ?? UndefinedStationName,
            FromDate = DateTimeOffset.FromUnixTimeMilliseconds(clientResponse.Content?.Period?.From ?? 0).UtcDateTime,
            ToDate = DateTimeOffset.FromUnixTimeMilliseconds(clientResponse.Content?.Period?.To ?? 0).UtcDateTime
        };
    }

    /// <summary>
    /// Retrieves instantaneous temperatures for each available station for the latest hour.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<StationParameterDto>> GetInstantTemperaturePerStationAsync(CancellationToken cancellationToken)
    {
        // Fetches temperature data for all stations for the last hour
        var clientResponse = await stationSetMeteorologyClient.GetAsync(
            MeteorologyParameterKey.InstantTemperaturePerHour,
            StationSetKey.All,
            PeriodParameterKey.LatestHour,
            cancellationToken);

        // Ensure the HTTP response is successful.
        // Note: It is possible to implement a separate service for processing responses from a specific provider client.
        // This service can generate and throw the necessary types of exceptions,
        // which will then be caught at the main level (Middleware) before being displayed to the user.
        // At this time, we do not expect any errors that need to be handled in a special way.
        await clientResponse.EnsureSuccessStatusCodeAsync();

        // Aggregate and map response to a collection of data transfer objects.
        return clientResponse.Content?.Stations?
                   .Where(x => x.MeteorologyParameters?.Length > 0 && !string.IsNullOrWhiteSpace(x.MeteorologyParameters!.First().Value))
                   .Select(
                       x => new StationParameterDto
                       {
                           StationName = x.Name ?? UndefinedStationName,
                           Value = double.TryParse(x.MeteorologyParameters!.First().Value, CultureInfo.InvariantCulture, out var result)
                               ? result
                               : default
                       })
                   .ToList()
               ?? [];
    }
}
