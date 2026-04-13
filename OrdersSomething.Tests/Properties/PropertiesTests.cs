namespace OrdersSomething.Tests.Properties;

public class PropertiesTests(HttpClientFixture fixture) : PropertiesFixtures(fixture)
{
    [Fact]
    public async Task ShouldCreateNewProperty()
    {
        // given
        var command = CreatePropertyCommand();

        // when
        var upsertPropertyResponse = await CreateProperty(command);

        // then
        await TestHelper.WaitUntil(async () =>
        {
            var actualProperty = await GetPropertyById(upsertPropertyResponse.Id);

            PropertiesAssertions.AssertProperties(command, actualProperty);
        });
    }

    [Fact]
    public async Task ShouldModifyExistingProperty()
    {
        // given
        var existingPropertyCommand = CreatePropertyCommand();
        var existingProperty = await CreateProperty(existingPropertyCommand);

        var modifiedProperty = ModifyPropertyCommand(existingProperty.Id);

        // when
        await ModifyProperty(modifiedProperty);

        // then
        await TestHelper.WaitUntil(async () =>
        {
            var actualProperty = await GetPropertyById(modifiedProperty.Id);

            PropertiesAssertions.AssertProperties(modifiedProperty, actualProperty);
        });
    }

    [Fact]
    public async Task ShouldMarkPropertyAsDeleted()
    {
        // given
        var existingPropertyCommand = CreatePropertyCommand();
        var existingProperty = await CreateProperty(existingPropertyCommand);

        var deletePropertyCommand = DeletePropertyCommand(existingProperty.Id, true);

        // when
        await DeleteProperty(deletePropertyCommand);

        // then
        await TestHelper.WaitUntil(async () =>
        {
            var actualProperty = await GetPropertyById(deletePropertyCommand.Id);

            PropertiesAssertions.AssertProperties(deletePropertyCommand, actualProperty);
        });
    }

    [Fact]
    public async Task ShouldNotDeletePropertyAndThrowEntityNotFoundException()
    {
        // given
        var notExistingProperty = DeletePropertyCommand(Guid.NewGuid(), true);

        // when
        var response = await DeletePropertyHttpClient(notExistingProperty);

        // then
        PropertiesAssertions.AssertNotFound(response);
    }

    [Fact]
    public async Task ShouldNotGetPropertyAndThrowEntityNotFoundException()
    {
        // given
        var propertyId = Guid.NewGuid();

        // when
        var response = await GetPropertyByIdHttpClient(propertyId);

        // then
        PropertiesAssertions.AssertNotFound(response);
    }

    [Fact]
    public async Task ShouldNotModifyStatusAndThrowEntityNotFoundException()
    {
        // given
        var propertyId = Guid.NewGuid();

        // when
        var response = await GetPropertyByIdHttpClient(propertyId);

        // then
        PropertiesAssertions.AssertNotFound(response);
    }
}