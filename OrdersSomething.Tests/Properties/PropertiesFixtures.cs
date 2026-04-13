using OrdersSomething.Command.Api.Features.Properties.Commands;
using System.Net.Http.Json;
using OrdersSomething.Query.Api.Features.Properties;

namespace OrdersSomething.Tests.Properties;

public class PropertiesFixtures(HttpClientFixture fixture) : IClassFixture<HttpClientFixture>
{
    private static String PROPERTY_URL = "/api/Properties";

    internal static UpsertPropertyCommand CreatePropertyCommand()
    {
        return new UpsertPropertyCommand
        {
            Id = Guid.Empty,
            Name = "CQRS Test",
            Address = "Test Address",
            Description = "Created by CQRS test",
            IsDeleted = false
        };
    }

    internal static UpsertPropertyCommand ModifyPropertyCommand(Guid propertyId)
    {
        return new UpsertPropertyCommand
        {
            Id = propertyId,
            Name = "Modified CQRS Test",
            Address = "Modified Test Address",
            Description = "Modified by CQRS test",
            IsDeleted = true
        };
    }

    internal static DeletePropertyCommand DeletePropertyCommand(Guid propertyId, bool isDeleted)
    {
        return new DeletePropertyCommand
        {
            Id = propertyId,
            IsDeleted = isDeleted
        };
    }

    public async Task<UpsertPropertyResponse?> CreateProperty(UpsertPropertyCommand command)
    {
        var response = await fixture.CommandClient.PostAsJsonAsync(PROPERTY_URL, command);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<UpsertPropertyResponse>();
    }

    public async Task ModifyProperty(UpsertPropertyCommand command)
    {
        var response = await fixture.CommandClient.PutAsJsonAsync(PROPERTY_URL, command);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProperty(DeletePropertyCommand command)
    {
        var response = await fixture.CommandClient.PatchAsJsonAsync(PROPERTY_URL, command);
        response.EnsureSuccessStatusCode();
    }

    public async Task<HttpResponseMessage> DeletePropertyHttpClient(DeletePropertyCommand command)
    {
        return await fixture.CommandClient.PatchAsJsonAsync(PROPERTY_URL, command);
    }

    public async Task<PropertiesDto?> GetPropertyById(Guid id)
    {
        var response = await fixture.QueryClient.GetAsync($"{PROPERTY_URL}/{id}");
        return await response.Content.ReadFromJsonAsync<PropertiesDto>();
    }

    public async Task<HttpResponseMessage> GetPropertyByIdHttpClient(Guid id)
    {
        return await fixture.QueryClient.GetAsync($"{PROPERTY_URL}/{id}");
    }
}