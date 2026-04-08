
namespace OrdersSomething.Tests;

public class CqrsE2EFixture : IDisposable
{
    public HttpClient CommandClient { get; }
    public HttpClient QueryClient { get; }

    public CqrsE2EFixture()
    {
        CommandClient = new HttpClient { BaseAddress = new Uri("http://localhost:5078") };
        QueryClient = new HttpClient { BaseAddress = new Uri("http://localhost:5284") };
    }

    public void Dispose()
    {
        CommandClient.Dispose();
        QueryClient.Dispose();
    }
}