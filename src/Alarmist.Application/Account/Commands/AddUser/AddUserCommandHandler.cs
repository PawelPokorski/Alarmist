using Alarmist.Application.Common;
using Alarmist.Application.Common.Commands;
using Alarmist.Application.DTOs;
using Alarmist.Domain.Entities;
using Alarmist.Domain.Interfaces;
using AutoMapper;

namespace Alarmist.Application.Account.Commands.AddUser;

public class AddUserCommandHandler(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork) : ICommandHandler<AddUserCommand>
{
    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if(await userRepository.FindByEmail(request.Email, cancellationToken) != null)
        {
            return Result.Failure("Email already in use.");
        }

        var user = User.Create(request.Email, request.Password);

        userRepository.Add(user);

        await unitOfWork.CommitChanges(cancellationToken);

        return Result.Success(mapper.Map<UserDto>(user));
    }
} 