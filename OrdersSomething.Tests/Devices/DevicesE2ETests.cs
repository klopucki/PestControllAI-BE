using OrdersSomething.Tests.Devices;

namespace OrdersSomething.Tests;

public class DevicesE2ETests(HttpClientFixture fixture) : DevicesFixtures(fixture)
{
    [Fact]
    public async Task ShouldCreateNewDevice()
    {
        // given
        var command = CreateDeviceCommand();

        // when
        var upsertDeviceResponse = await CreateDevice(command);

        // then
        await TestHelper.WaitUntil(async () =>
        {
            var actualDevice = await GetDeviceById(upsertDeviceResponse.Id);

            DevicesAssertions.AssertDevice(command, actualDevice);
        });
    }

    [Fact]
    public async Task ShouldModifyExistingDevice()
    {
        // given
        var existingDeviceCommand =  CreateDeviceCommand();;
        var existingDevice = await CreateDevice(existingDeviceCommand);

        var modifiedDevice = ModifyDeviceCommand(existingDevice.Id);

        // when
        await ModifyDevice(modifiedDevice);

        // then
        await TestHelper.WaitUntil(async () =>
        {
            var actualDevice = await GetDeviceById(modifiedDevice.Id);

            DevicesAssertions.AssertDevice(modifiedDevice, actualDevice);
        });
    }

    [Fact]
    public async Task ShouldMarkDeviceAsDeleted()
    {
        // given
        var existingDeviceCommand = CreateDeviceCommand();
        var existingDevice = await CreateDevice(existingDeviceCommand);

        var deleteDeviceCommand = DeleteDeviceCommand(existingDevice.Id, true);

        // when
        await DeleteDevice(deleteDeviceCommand);

        // then
        await TestHelper.WaitUntil(async () =>
        {
            var actualDevice = await GetDeviceById(existingDevice.Id);

            DevicesAssertions.AssertDevice(deleteDeviceCommand, actualDevice);
        });
    }

    [Fact]
    public async Task ShouldStopListetningDevice()
    {
        // given
        var existingDeviceCommand = CreateDeviceCommand();
        var existingDevice = await CreateDevice(existingDeviceCommand);

        var updateListeningCommand = UpdateListeningDeviceCommand(existingDevice.Id, false);

        // when
        await UpdateListeningDevice(updateListeningCommand);

        // then
        await TestHelper.WaitUntil(async () =>
        {
            var actualDevice = await GetDeviceById(existingDevice.Id);

            DevicesAssertions.AssertDevice(updateListeningCommand, actualDevice);
        });
    }

    [Fact]
    public async Task ShouldNotDeviceDeleteAndThrowEntityNotFoundException()
    {
        // given
        var notExistingDevice = DeleteDeviceCommand(Guid.NewGuid(), true);

        // when
        var response = await DeleteDeviceHttpClient(notExistingDevice);

        // then
        DevicesAssertions.AssertNotFound(response);
    }
    
    [Fact]
    public async Task ShouldNotGetDeviceAndThrowEntityNotFoundException()
    {
        // given
        var propertyId = Guid.NewGuid();

        // when
        var response = await GetDeviceByIdHttpClient(propertyId);

        // then
        DevicesAssertions.AssertNotFound(response);
    }
}