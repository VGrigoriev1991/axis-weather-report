using Axis.WeatherReport.ConsoleApp.Models;
using Refit;

namespace Axis.WeatherReport.ConsoleApp.Clients;

/// <summary>
/// Interface definition for a client to interact with meteorological data for a set of stations.
/// </summary>
public interface IStationSetMeteorologyClient
{
    [Get("/parameter/{parameter}/station-set/{stationSet}/period/{period}/data.json")]
    Task<ApiResponse<StationSetMeteorology>> GetAsync(int parameter, string stationSet, string period, CancellationToken cancellationToken);
}
