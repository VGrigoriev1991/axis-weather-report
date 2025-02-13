using Axis.WeatherReport.ConsoleApp.Services;

namespace Axis.WeatherReport.ConsoleApp.Workers;

public class AverageTemperatureWorker(IMeteorologyService meteorologyService) : IWorker
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var averageTemperature = await meteorologyService.CalculateLatestHourAverageTemperatureAsync(cancellationToken);

        Console.WriteLine($"The average temperature in Sweden for the last hours was {averageTemperature} degrees");
    }
}
