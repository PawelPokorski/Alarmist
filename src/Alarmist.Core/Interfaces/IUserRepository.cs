using Alarmist.Core.Models;

namespace Alarmist.Core.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserById(string id);
    
    void Add(User user);
    void Update(User user);
    void Delete(User user);
}