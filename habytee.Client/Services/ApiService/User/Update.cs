using System.Net.Http.Json;
using habytee.Interconnection.Models;
using Habytee.Interconnection.Dto;

namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<User?> UpdateUserAsync(UpdateUserDto user)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync<UpdateUserDto>($"/api/user", user);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating user: {error}");
                return null;
            }
            return await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
