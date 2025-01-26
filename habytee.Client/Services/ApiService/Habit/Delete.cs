namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<bool> DeleteHabitAsync(int id)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"/api/habit/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
