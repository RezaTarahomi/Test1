using AutoMapper;
using CodeGenerator.Web.Models;
using Database.Data.Entities;

namespace CodeGenerator.Web.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Entity, EntityViewModel>().ReverseMap();
            CreateMap<Field, FieldViewModel>().ReverseMap();
        }

    }
}
