using System.Net.Http.Json;
using habytee.Interconnection.Models;

namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<bool> UpdateHabitAsync(int id, Habit habit)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"/api/habit/{id}", habit);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
