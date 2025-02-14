using System.Text.Json;
using System.Text.Json.Serialization;
using Axis.WeatherReport.ConsoleApp.Clients;
using Axis.WeatherReport.ConsoleApp.Configurations;
using Axis.WeatherReport.ConsoleApp.Services;
using Axis.WeatherReport.ConsoleApp.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Refit;

// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to enhance IServiceCollection with specific dependencies.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Method to add all required application dependencies to the service container.
    /// This method centrally configures and registers all the dependencies necessary
    /// for the application to function.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        // Set up configuration from the "appsettings.json" file.
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Configure and bind settings for the REST Client that interfaces with the Meteorology API,
        // taking necessary configuration from the dedicated section "MeteorologyClient" in appsettings.json.
        services.AddOptions<RestClientSettings>()
            .Bind(configuration.GetSection("MeteorologyClient"));

        // Register the specific meteorology data provider service.
        services.AddSmhiProvider();

        // Register business logic services.
        services.AddServices();

        // Register all workers that are responsible for displaying the user interface part of the application.
        services.AddWorkers();

        return services;
    }

    private static IServiceCollection AddSmhiProvider(this IServiceCollection services) => services.AddClients();

    private static IServiceCollection AddClients(this IServiceCollection services) =>
        services
            .AddClient<IStationSetMeteorologyClient>()
            .AddClient<IStationMeteorologyClient>();

    private static IServiceCollection AddClient<TClient>(this IServiceCollection services)
        where TClient : class
    {
        services
            .AddRefitClient<TClient>(GetRefitSettings())
            .ConfigureHttpClient(
                (service, client) =>
                {
                    var settings = service.GetRequiredService<IOptions<RestClientSettings>>();
                    client.BaseAddress = new Uri(settings.Value.BaseUrl);
                });

        return services;
    }

    private static RefitSettings GetRefitSettings() =>
        new()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                })
        };

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddTransient<IMeteorologyService, MeteorologyService>();

    private static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddKeyedTransient<IWorker, AverageTemperatureWorker>(WorkerKey.AverageTemperature);
        services.AddKeyedTransient<IWorker, TotalRainfallWorker>(WorkerKey.TotalRainfall);
        services.AddKeyedTransient<IWorker, InstantTemperatureWorker>(WorkerKey.InstantTemperature);

        return services;
    }
}
