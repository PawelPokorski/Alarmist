using Alarmist.Domain.Entities;
using Alarmist.Domain.Interfaces;
using Alarmist.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Alarmist.Infrastructure.Persistence.Repositories;

public class UserRepository(AlarmistContext context) : IUserRepository
{
    public async Task<User> FindById(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> FindByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users.ToListAsync(cancellationToken);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public void Update(User user)
    {
        context.Users.Update(user);
    }

    public void Delete(User user)
    {
        context.Users.Remove(user);
    }
}