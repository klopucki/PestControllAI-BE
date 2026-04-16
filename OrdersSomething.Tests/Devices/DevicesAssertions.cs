using System.Net;
using FluentAssertions;
using OrdersSomething.Command.Api.Features.Devices.Commands;
using OrdersSomething.Query.Api.Features.Devices;
using OrdersSomething.Query.Api.Features.Properties;

namespace OrdersSomething.Tests.Devices;

internal class DevicesAssertions
{
    internal static void AssertDevice(UpsertDeviceCommand expected, DeviceDto? actual)
    {
        actual.Should()
            .BeEquivalentTo(expected, options => options
                    .Excluding(x => x.Id)
                    .Excluding(x => x.LastHeartbeat) // fixme date
                    .Excluding(x => x.CreatedAt) // fixme date
            );
    }

    internal static void AssertDevice(DeleteDeviceCommand expected, DeviceDto? actual)
    {
        actual.IsDeleted.Should().Be(expected.IsDeleted);
    }

    public static void AssertNotFound(HttpResponseMessage response)
    {
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public static void AssertDevice(UpdateListeningCommand expected, DeviceDto? actual)
    {
        actual.IsDeleted.Should().Be(expected.IsListening);
    }
}