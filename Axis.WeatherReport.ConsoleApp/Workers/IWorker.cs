namespace Axis.WeatherReport.ConsoleApp.Workers;

public interface IWorker
{
    Task RunAsync(CancellationToken cancellationToken);
}
