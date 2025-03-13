using Alarmist.Application.Common.Queries;
using Alarmist.Application.DTOs;
using Alarmist.Domain.Interfaces;
using AutoMapper;

namespace Alarmist.Application.Account.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindById(request.Id, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return mapper.Map<UserDto>(user);
    }
}