namespace VOD.Common.HttpClients;

public class MembershipHttpClient
{
    public HttpClient Client { get; }

    public MembershipHttpClient(HttpClient httpClient)
    {
        Client = httpClient;
    }
}
