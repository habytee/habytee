using habytee.Interconnection.Models;

namespace habytee.Server.DataAccess;

public interface IDataService
{ 
    public User GetReadUser(string email);
}
