using Alarmist.Application.Common.Queries;
using Alarmist.Application.DTOs;

namespace Alarmist.Application.Account.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto>;
