using Xunit.Sdk;

namespace OrdersSomething.Tests;

public static class TestHelper
{
    /// <summary>
    /// Wait until assertion success or timeout exceeded.
    /// </summary>
    /// <param name="action">Assert function</param>
    /// <param name="timeoutSeconds">Timeout in seconds with default value 15s</param>
    public static async Task WaitUntil(Func<Task> action, int timeoutSeconds = 15)
    {
        var start = DateTime.UtcNow;
        Exception? lastException = null;

        while (DateTime.UtcNow - start < TimeSpan.FromSeconds(timeoutSeconds))
        {
            try
            {
                await action();
                return; // Sukces! Asercje przeszły.
            }
            catch (Exception ex)
            {
                lastException = ex;
                await Task.Delay(500); // Czekaj przed ponowieniem
            }
        }

        throw new XunitException($"Timeout: Assertion failed within {timeoutSeconds}s. Last error: {lastException?.Message}", lastException);
    }
}
