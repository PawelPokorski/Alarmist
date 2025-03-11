using AutoMapper;
using Alarmist.Application.DTOs;
using Alarmist.Domain.Entities;

namespace Alarmist.Application.Common.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
    }
} 