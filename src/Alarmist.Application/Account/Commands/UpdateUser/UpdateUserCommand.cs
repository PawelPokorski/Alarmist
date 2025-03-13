using Alarmist.Application.Common.Commands;
using Alarmist.Application.DTOs;

namespace Alarmist.Application.Account.Commands.UpdateUser;

public record UpdateUserCommand(UserDto UserDto) : ICommand;