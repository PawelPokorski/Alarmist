using Alarmist.Application.Common;
using Alarmist.Application.Common.Commands;
using Alarmist.Domain.Entities;
using Alarmist.Domain.Interfaces;

namespace Alarmist.Application.Account.Commands.AddUser;

public class AddUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddUserCommand>
{
    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if(await userRepository.FindByEmail(request.Email) != null)
        {
            return Result.Failure("Email already in use.");
        }

        var user = User.Create(request.Email, request.Password);

        userRepository.Add(user);

        await unitOfWork.CommitChanges(cancellationToken);

        return Result.Success();
    }
} 