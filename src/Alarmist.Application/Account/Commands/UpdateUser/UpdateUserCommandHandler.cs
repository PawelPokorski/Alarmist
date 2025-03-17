using Alarmist.Application.Common;
using Alarmist.Application.Common.Commands;
using Alarmist.Domain.Interfaces;
using AutoMapper;

namespace Alarmist.Application.Account.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userDto = request.UserDto;
        var user = await userRepository.FindById(userDto.Id, cancellationToken);

        if(user == null)
        {
            return Result.Failure("User not found.");
        }

        user.Email = userDto.Email;
        user.PasswordHash = userDto.PasswordHash;
        user.DisplayName = userDto.DisplayName;
        user.EmailVerified = userDto.EmailVerified;
        user.VerificationCode = userDto.VerificationCode;
        user.RecoveryId = userDto.RecoveryId;

        userRepository.Update(user);
        await unitOfWork.CommitChanges(cancellationToken);

        return Result.Success();
    }
}