namespace Alarmist.Domain.Interfaces;

public interface IUnitOfWork
{
    Task CommitChanges(CancellationToken cancellationToken = default);
}