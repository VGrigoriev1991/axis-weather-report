// Entry point for the Console application working with weather data processing.

using Axis.WeatherReport.ConsoleApp.Workers;
using Microsoft.Extensions.DependencyInjection;

// Setting up ServiceProvider to organize dependency injection and managing application services.
var serviceProvider = new ServiceCollection()
    .AddApplicationDependencies()
    .BuildServiceProvider();

// Creation of a CancellationTokenSource to manage task cancellation.
var cancellationTokenSource = new CancellationTokenSource();

try
{
    // Asynchronous execution of background workers that process different meteorological data.
    await RunWorkersAsync(serviceProvider, cancellationTokenSource.Token);

    // Run a specific worker with the ability to cancel the operation based on user input.
    RunWorkerWithCancellation(serviceProvider, cancellationTokenSource);
}
catch (Exception)
{
    // Global exception handling to catch unforeseen errors.
    // We do not expect exceptions that need to be displayed to the user in any special way.
    // If such a need arises, it is necessary to write catch blocks for each specific type of exception.
    // For a more complex option, it is possible to implement a special error handler at this stage,
    // which will act as a middleware for error handling centrally.
    Console.WriteLine(
        "Unfortunately, an unexpected error occurred. Please contact the author of the code to resolve the problem.");
}

// Keep the console window open until a key is pressed.
Console.ReadLine();

return;

// Function responsible for running specific workers that process meteorological data.
async Task RunWorkersAsync(IServiceProvider provider, CancellationToken cancellationToken)
{
    var workers = new[]
    {
        provider.GetRequiredKeyedService<IWorker>(WorkerKey.AverageTemperature),
        provider.GetRequiredKeyedService<IWorker>(WorkerKey.TotalRainfall)
    };

    foreach (var worker in workers)
    {
        await worker.RunAsync(cancellationToken);
    }
}

// Function to run a worker with the capability to cancel the execution based on user input.
void RunWorkerWithCancellation(
    IServiceProvider provider,
    CancellationTokenSource cancellationToken)
{
    var worker = provider.GetRequiredKeyedService<IWorker>(WorkerKey.InstantTemperature);

    // Starting the worker asynchronously without awaiting its completion, allowing for immediate interaction.
    worker.RunAsync(cancellationToken.Token);

    // Listen for a key press to trigger the cancellation.
    Console.ReadKey();
    cancellationToken.Cancel(); // Signals a cancellation request to the worker.
}
