using habytee.Interconnection.Models;

namespace habytee.Server.DataAccess;

public class DataService(ReadDbContext readDbContext, WriteDbContext writeDbContext) : IDataService
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
