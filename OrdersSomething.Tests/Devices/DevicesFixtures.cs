using System.Net.Http.Json;
using OrdersSomething.Command.Api.Features.Devices.Commands;
using OrdersSomething.Query.Api.Features.Devices;

namespace OrdersSomething.Tests.Devices;

public class DevicesFixtures(HttpClientFixture fixture) : IClassFixture<HttpClientFixture>
{
    private static String DEVICE_URL = "/api/Devices";

    internal static UpsertDeviceCommand CreateDeviceCommand()
    {
        return new UpsertDeviceCommand
        {
            Id = Guid.Empty,
            Name = "CQRS Test",
            Type = "camera",
            Status = "active",
            IsListening = true,
            PropertiesId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            IsDeleted = false
        };
    }

    internal static UpsertDeviceCommand ModifyDeviceCommand(Guid deviceId)
    {
        return new UpsertDeviceCommand
        {
            Id = deviceId,
            Name = "Modified CQRS Test",
            Type = "microphone",
            Status = "inactive",
            IsListening = false,
            PropertiesId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            IsDeleted = false
        };
    }

    internal static DeleteDeviceCommand DeleteDeviceCommand(Guid deviceId, bool isDeleted)
    {
        return new DeleteDeviceCommand
        {
            Id = deviceId,
            IsDeleted = isDeleted
        };
    }

    public async Task<UpsertDeviceResponse?> CreateDevice(UpsertDeviceCommand command)
    {
        var response = await fixture.CommandClient.PostAsJsonAsync(DEVICE_URL, command);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UpsertDeviceResponse>();
    }

    public async Task ModifyDevice(UpsertDeviceCommand command)
    {
        var response = await fixture.CommandClient.PutAsJsonAsync(DEVICE_URL, command);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDevice(DeleteDeviceCommand command)
    {
        var response = await fixture.CommandClient.PatchAsJsonAsync(DEVICE_URL, command);
        response.EnsureSuccessStatusCode();
    }

    public async Task<HttpResponseMessage> DeleteDeviceHttpClient(DeleteDeviceCommand command)
    {
        return await fixture.CommandClient.PatchAsJsonAsync(DEVICE_URL, command);
    }

    public async Task<DeviceDto?> GetDeviceById(Guid id)
    {
        var response = await fixture.QueryClient.GetAsync($"{DEVICE_URL}/{id}");
        return await response.Content.ReadFromJsonAsync<DeviceDto>();
    }

    public async Task<HttpResponseMessage> GetDeviceByIdHttpClient(Guid id)
    {
        return await fixture.QueryClient.GetAsync($"{DEVICE_URL}/{id}");
    }
}