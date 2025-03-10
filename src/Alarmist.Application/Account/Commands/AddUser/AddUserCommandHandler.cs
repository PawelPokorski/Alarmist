using Alarmist.Application.Common;
using Alarmist.Application.Common.Commands;

namespace Alarmist.Application.Account.Commands.AddUser;

public class AddUserCommandHandler : ICommandHandler<AddUserCommand>
{
    public Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
} 