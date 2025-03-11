using AutoMapper;
using Alarmist.Application.Common.Queries;
using Alarmist.Application.DTOs;
using Alarmist.Domain.Interfaces;

namespace Alarmist.Application.Account.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler(IUserRepository userRepository, IMapper mapper) : IQueryHandler<GetUserByEmailQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmail(request.Email, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return mapper.Map<UserDto>(user);
    }
}