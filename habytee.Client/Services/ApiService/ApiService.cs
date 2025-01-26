namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    private readonly HttpClient httpClient;
    public ApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
}
