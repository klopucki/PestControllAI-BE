using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using OrdersSomething.Query.Api.Features.Devices;
using Xunit;

namespace OrdersSomething.Tests;

public class DevicesE2ETests(LocalDatabaseWebApplicationFactory factory) : IClassFixture<LocalDatabaseWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetByPropertyId_GivenExistingPropertyId_ShouldReturnListOfDevices()
    {
        // Given
        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");

        // When & Then
        await TestHelper.WaitUntil(async () =>
        {
            var response = await _client.GetAsync($"/api/Devices/property/{propertyId}");
            if (response.StatusCode != HttpStatusCode.OK) return false;
            
            var devices = await response.Content.ReadFromJsonAsync<List<DeviceDto>>();
            return devices != null && devices.Any();
        }, "Devices should be returned for the property");
    }

    [Fact]
    public async Task CreateDevice_GivenValidData_ShouldCreateNewDevice()
    {
        // Given
        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");
        var command = new UpsertDeviceCommand
        {
            Id = Guid.Empty,
            PropertyId = propertyId,
            Name = "E2E Test Device " + Guid.NewGuid().ToString()[..8],
            Type = "camera",
            Status = "active",
            IsListening = true,
            IsDeleted = false
        };

        // When
        var response = await _client.PostAsJsonAsync("/api/Devices", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Then
        await TestHelper.WaitUntil(async () =>
        {
            var getResponse = await _client.GetAsync($"/api/Devices/property/{propertyId}");
            if (getResponse.StatusCode != HttpStatusCode.OK) return false;
            
            var devices = await getResponse.Content.ReadFromJsonAsync<List<DeviceDto>>();
            return devices != null && devices.Any(d => d.Name == command.Name);
        }, "New device should be visible in the property device list");
    }

    [Fact]
    public async Task ModifyDevice_GivenExistingDevice_ShouldUpdateData()
    {
        // Given
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Then
        await TestHelper.WaitUntil(async () =>
        {
            var getResponse = await _client.GetAsync($"/api/Devices/property/{propertyId}");
            if (getResponse.StatusCode != HttpStatusCode.OK) return false;
            
            var devices = await getResponse.Content.ReadFromJsonAsync<List<DeviceDto>>();
            var updated = devices?.FirstOrDefault(d => d.Id == targetDeviceId);
            return updated != null && updated.Name == command.Name && updated.Status == command.Status;
        }, "Device data should be updated");
    }

    [Fact]
    public async Task DeleteDevice_GivenExistingDevice_ShouldMarkAsDeleted()
    {
        // Given
        var targetDeviceId = new Guid("A2222222-2222-2222-2222-222222222222");
        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");
        var command = new DeleteDeviceCommand
        {
            Id = targetDeviceId,
            IsDeleted = true
        };

        // When
        var response = await _client.PatchAsJsonAsync("/api/Devices", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Then
        await TestHelper.WaitUntil(async () =>
        {
            var getResponse = await _client.GetAsync($"/api/Devices/property/{propertyId}");
            if (getResponse.StatusCode != HttpStatusCode.OK) return false;
            
            var devices = await getResponse.Content.ReadFromJsonAsync<List<DeviceDto>>();
            var device = devices?.FirstOrDefault(d => d.Id == targetDeviceId);
            return device != null && device.IsDeleted == true;
        }, "Device should be marked as deleted");
    }

    [Fact]
    public async Task UpdateListeningDevice_GivenExistingDevice_ShouldChangeListeningStatus()
    {
        // Given
        var targetDeviceId = new Guid("A1111111-1111-1111-1111-111111111111");
        var propertyId = new Guid("11111111-1111-1111-1111-111111111111");
        var command = new UpdateListeningCommand
        {
            Id = targetDeviceId,
            IsListening = true
        };

        // When
        var response = await _client.PatchAsJsonAsync("/api/Devices/listening", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Then
        await TestHelper.WaitUntil(async () =>
        {
            var getResponse = await _client.GetAsync($"/api/Devices/property/{propertyId}");
            if (getResponse.StatusCode != HttpStatusCode.OK) return false;
            
            var devices = await getResponse.Content.ReadFromJsonAsync<List<DeviceDto>>();
            var device = devices?.FirstOrDefault(d => d.Id == targetDeviceId);
            return device != null && device.IsListening == true;
        }, "Device listening status should be updated");
    }

    [Fact]
    public async Task GetEventsByDeviceId_GivenExistingDeviceId_ShouldReturnListOfEvents()
    {
        // Given
        var deviceId = new Guid("A1111111-1111-1111-1111-111111111111");

        // When & Then
        await TestHelper.WaitUntil(async () =>
        {
            var response = await _client.GetAsync($"/api/Devices/{deviceId}/events");
            if (response.StatusCode != HttpStatusCode.OK) return false;
            
            var events = await response.Content.ReadFromJsonAsync<List<DeviceEventsDto>>();
            return events != null && events.Any();
        }, "Events should be returned for the device");
    }
}
