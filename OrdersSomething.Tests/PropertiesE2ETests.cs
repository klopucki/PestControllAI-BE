using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using OrdersSomething.Features.Properties.Commands;
using OrdersSomething.Features.Properties.Queries;
using Xunit;

namespace OrdersSomething.Tests;

public class PropertiesE2ETests(LocalDatabaseWebApplicationFactory factory) : IClassFixture<LocalDatabaseWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAllProperties_GivenExistingData_ShouldReturnListOfProperties()
    {
        // Given - seeded data -- todo change it
        
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
        // Given - seeded data -- todo change it
        var validId = new Guid("11111111-1111-1111-1111-111111111111");

        // When
        var response = await _client.GetAsync($"/api/Properties/{validId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var property = await response.Content.ReadFromJsonAsync<PropertiesDto>();
        property.Should().NotBeNull();
        property!.Id.Should().Be(validId);
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

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var allResponse = await _client.GetAsync("/api/Properties");
        var all = await allResponse.Content.ReadFromJsonAsync<List<PropertiesDto>>();
        all.Should().Contain(p => p.Name == command.Name);
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

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify update
        var getResponse = await _client.GetAsync($"/api/Properties/{targetId}");
        var updated = await getResponse.Content.ReadFromJsonAsync<PropertiesDto>();
        updated!.Name.Should().Be(command.Name);
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

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await _client.GetAsync($"/api/Properties/{targetId}");
        var property = await getResponse.Content.ReadFromJsonAsync<PropertiesDto>();
        property!.IsDeleted.Should().BeTrue();
    }
}
