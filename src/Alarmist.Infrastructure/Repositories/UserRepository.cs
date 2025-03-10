using Alarmist.Domain.Entities;
using Alarmist.Domain.Interfaces;
using Alarmist.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Alarmist.Infrastructure.Repositories;

public class UserRepository(AlarmistContext context, IUnitOfWork unitOfWork) : IUserRepository
{
    public async Task<User> FindByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
        unitOfWork.CommitChanges();
    }

    public void Update(User user)
    {
        context.Users.Update(user);
        unitOfWork.CommitChanges();
    }

    public void Delete(User user)
    {
        context.Users.Remove(user);
        unitOfWork.CommitChanges();
    }
}