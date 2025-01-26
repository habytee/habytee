using System.Net.Http.Json;
using habytee.Interconnection.Models;

namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<HabitCheckedEvent?> CreateHabitCheckedEventAsync(int habitId)
    {
        try
        {
            var response = await httpClient.PostAsync($"/api/habit/{habitId}/checkedEvent", null);
            return await response.Content.ReadFromJsonAsync<HabitCheckedEvent>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
