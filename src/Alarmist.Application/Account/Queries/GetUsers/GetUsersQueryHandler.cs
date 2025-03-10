using Alarmist.Application.Account.DTOs;
using Alarmist.Application.Common.Queries;

namespace Alarmist.Application.Account.Queries.GetUsers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implementacja logiki pobierania użytkowników
        // 1. Pobranie użytkowników z bazy
        // 2. Mapowanie na DTOs
        // 3. Zwrócenie wyników
        
        return new List<UserDto>();
    }
}
