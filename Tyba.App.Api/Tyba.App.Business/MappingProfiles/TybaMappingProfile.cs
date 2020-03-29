using AutoMapper;
using Tyba.App.Business.Models;
using Tyba.App.Persistence.Entities;

namespace Tyba.App.Business.MappingProfiles
{
    public class TybaMappingProfile : Profile
    {
        public TybaMappingProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
