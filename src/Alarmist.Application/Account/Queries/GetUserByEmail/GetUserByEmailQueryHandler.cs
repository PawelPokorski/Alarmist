// using AutoMapper;
using Alarmist.Application.Common.Queries;
using Alarmist.Application.DTOs;
using Alarmist.Domain.Entities;
using Alarmist.Domain.Interfaces;

namespace Alarmist.Application.Account.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByEmailQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmail(request.Email, cancellationToken);

        // Add mapper

        // var userDto = mapper.Map<UserDto>(user);

        //return userDto;

        return null;
    }
}