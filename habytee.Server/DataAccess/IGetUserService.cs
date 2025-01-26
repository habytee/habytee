using habytee.Interconnection.Models;

namespace habytee.Server.DataAccess;

public interface IGetUserService
{ 
    public User GetReadUser(string email);
}
