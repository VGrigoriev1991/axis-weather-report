using Axis.WeatherReport.ConsoleApp.Services;

namespace Axis.WeatherReport.ConsoleApp.Workers;

public class TotalRainfallWorker(IMeteorologyService meteorologyService) : IWorker
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        // The station identifier can be turned into a configuration setting if necessary and described in a configuration file.
        // A constant is used for simplification.
        // If necessary, each worker can have its own configuration settings in the configuration file.
        // These settings can be injected into each worker ctor as IOptions<WorkerSettings>.
        var totalRainfall = await meteorologyService.CalculateTotalRainfallAsync("53430", cancellationToken);

        Console.WriteLine(
            $"Between {totalRainfall.FromDate.Date:yyyy-MM-dd} and {totalRainfall.ToDate.Date:yyyy-MM-dd} "
            + $"the total rainfall in {totalRainfall.StationName} was {totalRainfall.Value} millimeters");
    }
}
