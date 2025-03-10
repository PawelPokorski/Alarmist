using Alarmist.Application.Common.Queries;
using Alarmist.Application.DTOs;

namespace Alarmist.Application.Account.Queries.GetUsers;

public record GetUsersQuery : IQuery<IEnumerable<UserDto>>;
