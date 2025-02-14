using System.Net;
using Axis.WeatherReport.ConsoleApp.Models;
using Axis.WeatherReport.ConsoleApp.Services;
using FluentAssertions;
using NSubstitute;
using Refit;

namespace Axis.WeatherReport.ConsoleApp.UnitTests.Services;

public partial class MeteorologyServiceTests
{
    [Fact]
    public async Task CalculateAsync_ValidApiResponse_ExpectedCalculatedResultReturned()
    {
        // Arrange
        _stationSetMeteorologyClientMock
            .GetAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(
                Create(
                    HttpStatusCode.OK,
                    new StationSetMeteorology
                    {
                        Stations =
                        [
                            new Station { MeteorologyParameters = [new MeteorologyParameter { Value = "1.5" }] },
                            new Station { MeteorologyParameters = [new MeteorologyParameter { Value = null }] },
                            new Station { MeteorologyParameters = [new MeteorologyParameter { Value = "2.7" }] }
                        ]
                    }));

        var service = new MeteorologyService(_stationSetMeteorologyClientMock, _stationMeteorologyClientMock);

        // Act
        var result = await service.CalculateLatestHourAverageTemperatureAsync(CancellationToken.None);

        // Assert
        result.Should().Be(2.1);
    }

    [Fact]
    public async Task CalculateAsync_ValidApiResponseWithoutData_ExpectedDefaultResultReturned()
    {
        // Arrange
        _stationSetMeteorologyClientMock
            .GetAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(
                Create(
                    HttpStatusCode.OK,
                    new StationSetMeteorology
                    {
                        Stations =
                        [
                            new Station { MeteorologyParameters = [new MeteorologyParameter { Value = null }] },
                            new Station { MeteorologyParameters = [new MeteorologyParameter { Value = null }] }
                        ]
                    }));

        var service = new MeteorologyService(_stationSetMeteorologyClientMock, _stationMeteorologyClientMock);

        // Act
        var result = await service.CalculateLatestHourAverageTemperatureAsync(CancellationToken.None);

        // Assert
        result.Should().Be(default);
    }

    private static ApiResponse<TContent> Create<TContent>(
        HttpStatusCode responseStatusCode,
        TContent responseContent)
        where TContent : class
    {
        var refitSettings = new RefitSettings();
        var httpResponseMessage = new HttpResponseMessage(responseStatusCode);

        var response = new ApiResponse<TContent>(httpResponseMessage, responseContent, refitSettings);

        return response;
    }
}
