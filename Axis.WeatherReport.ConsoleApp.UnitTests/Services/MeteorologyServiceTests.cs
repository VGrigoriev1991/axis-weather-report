using Axis.WeatherReport.ConsoleApp.Clients;
using NSubstitute;

namespace Axis.WeatherReport.ConsoleApp.UnitTests.Services;

public partial class MeteorologyServiceTests
{
    private readonly IStationSetMeteorologyClient _stationSetMeteorologyClientMock = Substitute.For<IStationSetMeteorologyClient>();
    private readonly IStationMeteorologyClient _stationMeteorologyClientMock = Substitute.For<IStationMeteorologyClient>();
}
