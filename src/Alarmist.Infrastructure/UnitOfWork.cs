using Alarmist.Domain.Interfaces;
using Alarmist.Infrastructure.Persistence.Data;

namespace Alarmist.Infrastructure;

public class UnitOfWork(AlarmistContext context) : IUnitOfWork
{
    public async Task CommitChanges(CancellationToken cancellationToken = default)
    {
        // Dodatkowa obsługa zdarzeń przed zapisaniem zmian

        await context.SaveChangesAsync(cancellationToken);
    }
}