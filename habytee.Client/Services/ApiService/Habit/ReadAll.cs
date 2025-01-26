using System.Net.Http.Json;
using habytee.Interconnection.Models;

namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<List<Habit>?> GetAllHabitsAsync()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<List<Habit>>("/api/habit");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
