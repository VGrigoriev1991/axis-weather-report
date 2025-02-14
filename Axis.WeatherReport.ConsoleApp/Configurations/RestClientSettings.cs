namespace Axis.WeatherReport.ConsoleApp.Configurations;

/// <summary>
/// Class to define settings for REST API clients.
/// </summary>
public class RestClientSettings
{
    /// <summary>
    /// Property representing the base URL of the REST API.
    /// </summary>
    public required string BaseUrl { get; set; }
}
