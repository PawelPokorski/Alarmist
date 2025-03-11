using Alarmist.Application.Common.Queries;
using Alarmist.Application.DTOs;
using Alarmist.Domain.Interfaces;
using AutoMapper;

namespace Alarmist.Application.Account.Queries.GetUsers;

public class GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IQueryHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);

        var userDtos = mapper.Map<IEnumerable<UserDto>>(users);

        return userDtos;
    }
}