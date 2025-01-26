using habytee.Interconnection.Models;

namespace habytee.Server.DataAccess;

public class GetUserService(ReadDbContext readDbContext, WriteDbContext writeDbContext) : IGetUserService
{
    private readonly ReadDbContext readDbContext = readDbContext;
    private readonly WriteDbContext writeDbContext = writeDbContext;

    public User GetReadUser(string email)
    {
        var readUser = readDbContext.Users.FirstOrDefault(u => u.Email == email);
        if(readUser == null)
        {
            readUser = new User()
            {
                Email = email
            };
            writeDbContext.Users.Add(readUser);
            writeDbContext.SaveChanges();
        }

        return readUser;
    }
}
