using System.Net.Http.Json;
using habytee.Interconnection.Models;

namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<User?> GetUserAsync()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<User>($"/api/user");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
