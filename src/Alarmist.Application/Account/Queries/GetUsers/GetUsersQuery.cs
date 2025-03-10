using Alarmist.Application.Account.DTOs;
using Alarmist.Application.Common.Queries;

namespace Alarmist.Application.Account.Queries.GetUsers;

public record GetUsersQuery() : IQuery<IEnumerable<UserDto>>;
