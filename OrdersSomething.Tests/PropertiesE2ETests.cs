using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using OrdersSomething.Command.Api.Features.Properties.Commands;
using OrdersSomething.Query.Api.Features.Properties;
using Xunit;

namespace OrdersSomething.Tests;

public class PropertiesE2ETests(CqrsE2EFixture fixture) : IClassFixture<CqrsE2EFixture>
{
    [Fact]
    public async Task CreateProperty_ShouldSynchronizeToQueryApi()
    {
        // 1. GIVEN - Nowa posesja
        var command = new UpsertPropertyCommand 
        { 
            Id = Guid.NewGuid(), 
            Name = "CQRS Test", 
            Address = "Test Address",
            Description = "Created by CQRS test",
            IsDeleted = false
        };

        // 2. WHEN - Wysyłamy do COMMAND
        var response = await fixture.CommandClient.PostAsJsonAsync("/api/Properties", command);
        response.EnsureSuccessStatusCode();

        // 3. THEN - Czekamy i sprawdzamy w QUERY
        await TestHelper.WaitUntil(async () => 
        {
            var queryResponse = await fixture.QueryClient.GetAsync($"/api/Properties/{command.Id}");
            if (queryResponse.StatusCode != HttpStatusCode.OK) return false;

            var property = await queryResponse.Content.ReadFromJsonAsync<PropertiesDto>();
            return property != null && property.Name == command.Name;
        }, "Property should be synchronized to Query API");
    }
    
    /*
    [Fact]
    public async Task GetAllProperties_GivenExistingData_ShouldReturnListOfProperties()
    {
        // When
        var response = await _client.GetAsync("/api/Properties");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var properties = await response.Content.ReadFromJsonAsync<List<PropertiesDto>>();
        properties.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPropertyById_GivenValidId_ShouldReturnProperty()
    {
        // Given
        var validId = new Guid("11111111-1111-1111-1111-111111111111");

        // When & Then
        await TestHelper.WaitUntil(async () =>
        {
            var response = await _client.GetAsync($"/api/Properties/{validId}");
            if (response.StatusCode != HttpStatusCode.OK) return false;
            
            var property = await response.Content.ReadFromJsonAsync<PropertiesDto>();
            return property != null && property.Id == validId;
        }, "Property should be returned by Id");
    }

    [Fact]
    public async Task CreateProperty_GivenValidData_ShouldCreateNewProperty()
    {
        // Given
        var command = new UpsertPropertyCommand
        {
            Id = Guid.Empty, 
            Name = "E2E Test Property " + Guid.NewGuid().ToString()[..8],
            Address = "Test Street 123",
            Description = "Created by E2E test",
            IsDeleted = false
        };

        // When
        var response = await _client.PostAsJsonAsync("/api/Properties", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Then
        await TestHelper.WaitUntil(async () =>
        {
            var allResponse = await _client.GetAsync("/api/Properties");
            var all = await allResponse.Content.ReadFromJsonAsync<List<PropertiesDto>>();
            return all != null && all.Any(p => p.Name == command.Name);
        }, "New property should be visible in the list");
    }

    [Fact]
    public async Task ModifyProperty_GivenExistingProperty_ShouldUpdateData()
    {
        // Given
        var targetId = new Guid("22222222-2222-2222-2222-222222222222");
        var command = new UpsertPropertyCommand
        {
            Id = targetId,
            Name = "Modified Name " + Guid.NewGuid().ToString()[..8],
            Address = "Updated Address",
            Description = "Updated by E2E test",
            IsDeleted = false
        };

        // When
        var response = await _client.PutAsJsonAsync("/api/Properties", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Then
        await TestHelper.WaitUntil(async () =>
        {
            var getResponse = await _client.GetAsync($"/api/Properties/{targetId}");
            if (getResponse.StatusCode != HttpStatusCode.OK) return false;
            
            var updated = await getResponse.Content.ReadFromJsonAsync<PropertiesDto>();
            return updated?.Name == command.Name;
        }, "Property name should be updated");
    }

    [Fact]
    public async Task UpdateStatus_GivenExistingProperty_ShouldMarkAsDeleted()
    {
        // Given
        var targetId = new Guid("33333333-3333-3333-3333-333333333333");
        var command = new DeletePropertyCommand
        {
            Id = targetId,
            IsDeleted = true
        };

        // When
        var response = await _client.PatchAsJsonAsync("/api/Properties", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Then
        await TestHelper.WaitUntil(async () =>
        {
            var getResponse = await _client.GetAsync($"/api/Properties/{targetId}");
            if (getResponse.StatusCode != HttpStatusCode.OK) return false;
            
            var property = await getResponse.Content.ReadFromJsonAsync<PropertiesDto>();
            return property?.IsDeleted == true;
        }, "Property should be marked as deleted");
    }*/
}
