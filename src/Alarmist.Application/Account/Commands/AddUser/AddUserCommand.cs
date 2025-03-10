using Alarmist.Application.Common.Commands;

namespace Alarmist.Application.Account.Commands.AddUser;

public record AddUserCommand(string Email, string Password) : ICommand;