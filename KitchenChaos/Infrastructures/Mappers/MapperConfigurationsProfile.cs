using AutoMapper;
using Application.Commons;
using Domain.Contracts.Abstract.Account;
using Application.ViewModels;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<RegisterUserRequest, RegisterUserDTO>();
        }
    }
}
