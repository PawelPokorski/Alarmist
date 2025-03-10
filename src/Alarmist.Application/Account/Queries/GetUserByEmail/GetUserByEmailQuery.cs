using Alarmist.Application.Common.Queries;
using Alarmist.Application.DTOs;

namespace Alarmist.Application.Account.Queries.GetUserByEmail;

public record GetUserByEmailQuery(string Email) : IQuery<UserDto>;