using Xunit.Sdk;

namespace OrdersSomething.Tests;

public static class TestHelper
{
    /// <summary>
    /// Helper method to wait for eventual consistency.
    /// Retries the provided condition for up to 15 seconds.
    /// </summary>
    public static async Task WaitUntil(Func<Task<bool>> condition, string message, int timeoutSeconds = 15)
    {
        var start = DateTime.UtcNow;
        while (DateTime.UtcNow - start < TimeSpan.FromSeconds(timeoutSeconds))
        {
            if (await condition()) return;
            await Task.Delay(500); // Wait 500ms between retries
        }
        throw new XunitException($"Timeout: {message} within {timeoutSeconds}s");
    }
}
