using Alarmist.Domain.Entities;

namespace Alarmist.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> FindByEmail(string email);

    void Add(User user);
    void Update(User user);
    void Delete(User user);
}