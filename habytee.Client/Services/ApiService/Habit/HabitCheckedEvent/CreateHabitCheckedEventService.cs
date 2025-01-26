namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<bool> CreateHabitCheckedEventAsync(int habitId)
    {
        try
        {
            var response = await httpClient.PostAsync($"/api/habit/{habitId}/checkedEvent", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
