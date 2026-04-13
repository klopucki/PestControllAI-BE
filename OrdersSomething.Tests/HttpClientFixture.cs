namespace OrdersSomething.Tests;

public class HttpClientFixture : IDisposable
{
    public HttpClient CommandClient { get; }
    public HttpClient QueryClient { get; }

    public HttpClientFixture()
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