namespace VOD.Common.HttpClients;

public class MemebershipHttpClient
{
    public HttpClient Client { get; }

    public MemebershipHttpClient(HttpClient httpClient)
    {
        Client = httpClient;
    }
}
