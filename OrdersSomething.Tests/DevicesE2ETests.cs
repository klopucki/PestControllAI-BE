using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using OrdersSomething.Features.Devices.Commands;
using OrdersSomething.Features.Devices.Query;
using OrdersSomething.Features.DeviceEvents.Queries;
using Xunit;

namespace OrdersSomething.Tests;

public class DevicesE2ETests(LocalDatabaseWebApplicationFactory factory) : IClassFixture<LocalDatabaseWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetByPropertyId_GivenExistingPropertyId_ShouldReturnListOfDevices()
    {
        // Given - seeded data -- todo change it
        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");

        // When
        var response = await _client.GetAsync($"/api/Devices/property/{propertyId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var devices = await response.Content.ReadFromJsonAsync<List<DeviceDto>>();
        devices.Should().NotBeNull();
        devices.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateDevice_GivenValidData_ShouldCreateNewDevice()
    {
        // Given
        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");
        var command = new UpsertDeviceCommand
        {
            Id = Guid.Empty, // Nowe urządzenie
            PropertyId = propertyId,
            Name = "E2E Test Device " + Guid.NewGuid().ToString()[..8],
            Type = "camera",
            Status = "active",
            IsListening = true,
            IsDeleted = false
        };

        // When
        var response = await _client.PostAsJsonAsync("/api/Devices", command);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var getResponse = await _client.GetAsync($"/api/Devices/property/{propertyId}");
        var devices = await getResponse.Content.ReadFromJsonAsync<List<DeviceDto>>();
        devices.Should().Contain(d => d.Name == command.Name);
    }

    [Fact]
    public async Task ModifyDevice_GivenExistingDevice_ShouldUpdateData()
    {
        // Given - seeded data -- todo change it
        var targetDeviceId = new Guid("A1111111-1111-1111-1111-111111111111");
        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");
        var command = new UpsertDeviceCommand
        {
            Id = targetDeviceId,
            PropertyId = propertyId,
            Name = "Modified Device Name " + Guid.NewGuid().ToString()[..8],
            Type = "camera",
            Status = "inactive",
            IsListening = false,
            IsDeleted = false
        };

        // When
        var response = await _client.PutAsJsonAsync("/api/Devices", command);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await _client.GetAsync($"/api/Devices/property/{propertyId}");
        var devices = await getResponse.Content.ReadFromJsonAsync<List<DeviceDto>>();
        var updated = devices!.First(d => d.Id == targetDeviceId);
        updated.Name.Should().Be(command.Name);
        updated.Status.Should().Be(command.Status);
    }

    [Fact]
    public async Task DeleteDevice_GivenExistingDevice_ShouldMarkAsDeleted()
    {
        // Given - seeded data -- todo change it
        var targetDeviceId = new Guid("A2222222-2222-2222-2222-222222222222");
        var command = new DeleteDeviceCommand
        {
            Id = targetDeviceId,
            IsDeleted = true
        };

        // When
        var response = await _client.PatchAsJsonAsync("/api/Devices", command);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");
        var getResponse = await _client.GetAsync($"/api/Devices/property/{propertyId}");
        var devices = await getResponse.Content.ReadFromJsonAsync<List<DeviceDto>>();
        devices!.First(d => d.Id == targetDeviceId).IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateListeningDevice_GivenExistingDevice_ShouldChangeListeningStatus()
    {
        // Given - seeded data -- todo change it
        var targetDeviceId = new Guid("A1111111-1111-1111-1111-111111111111");
        var command = new UpdateListeningCommand
        {
            Id = targetDeviceId,
            IsListening = true
        };

        // When
        var response = await _client.PatchAsJsonAsync("/api/Devices/listening", command);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");
        var getResponse = await _client.GetAsync($"/api/Devices/property/{propertyId}");
        var devices = await getResponse.Content.ReadFromJsonAsync<List<DeviceDto>>();
        devices!.First(d => d.Id == targetDeviceId).IsListening.Should().BeTrue();
    }

    [Fact]
    public async Task GetEventsByDeviceId_GivenExistingDeviceId_ShouldReturnListOfEvents()
    {
        // Given - seeded data -- todo change it
        var deviceId = new Guid("A1111111-1111-1111-1111-111111111111");

        // When
        var response = await _client.GetAsync($"/api/Devices/{deviceId}/events");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var events = await response.Content.ReadFromJsonAsync<List<DeviceEventsDto>>();
        events.Should().NotBeNull();
        events.Should().NotBeEmpty();
    }
}
