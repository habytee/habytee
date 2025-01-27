using habytee.Interconnection.Models;

namespace Habytee.Interconnection.Dto;

public class UpdateUserDto
{
    public int Coins { get; set; }

    public bool? LightTheme { get; set; }

    public string? Language { get; set; }

    public string? Currency { get; set; }

    public static UpdateUserDto CreateUpdateUserDto(User user)
    {
        return new UpdateUserDto { Coins = user.Coins, LightTheme = user.LightTheme, Language = user.Language, Currency = user.Currency };
    }
}
