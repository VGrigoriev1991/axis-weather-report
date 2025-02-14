using Axis.WeatherReport.ConsoleApp.Models;
using Refit;

namespace Axis.WeatherReport.ConsoleApp.Clients;

/// <summary>
/// Interface definition for a client to interact with meteorological data for a specific station.
/// </summary>
public interface IStationMeteorologyClient
{
    [Get("/parameter/{parameter}/station/{stationId}/period/{period}/data.json")]
    Task<ApiResponse<StationMeteorology>> GetAsync(int parameter, string stationId, string period, CancellationToken cancellationToken);
}
