namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<bool> DeleteHabitCheckedEventAsync(int habitId, int checkedEventId)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"/api/habit/{habitId}/checkedEvent/{checkedEventId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
