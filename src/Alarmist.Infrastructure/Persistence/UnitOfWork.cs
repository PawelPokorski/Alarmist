using Alarmist.Domain.Interfaces;
using Alarmist.Infrastructure.Persistence.Data;

namespace Alarmist.Infrastructure.Persistence;

public class UnitOfWork(AlarmistContext context) : IUnitOfWork
{
    public async Task CommitChanges(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}