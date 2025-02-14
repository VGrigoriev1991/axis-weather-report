using Axis.WeatherReport.ConsoleApp.Services;

namespace Axis.WeatherReport.ConsoleApp.Workers;

public class InstantTemperatureWorker(IMeteorologyService meteorologyService) : IWorker
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var instantTemperaturePerStation = await meteorologyService.GetInstantTemperaturePerStationAsync(cancellationToken);

        foreach (var instantTemperature in instantTemperaturePerStation)
        {
            // Checks if a cancellation request has been made to stop further execution.
            if (cancellationToken.IsCancellationRequested)
            {
                // Exit the loop if cancellation is requested.
                break;
            }

            Console.WriteLine($"{instantTemperature.StationName}: {instantTemperature.Value}");

            // Introduce a short delay to simulate processing time, making it easier to read or responsive to changes.
            // The delay is defined as 100 milliseconds here for simplicity.
            // This can be parameterized or moved to configuration settings if variability is needed.
            await Task.Delay(100, cancellationToken);
        }
    }
}
