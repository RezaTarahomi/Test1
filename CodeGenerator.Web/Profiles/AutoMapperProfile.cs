using AutoMapper;
using CodeGenerator.Core.Services.Dtos;
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
            CreateMap<EnumType, EnumTypeViewModel>().ReverseMap();
            CreateMap<EnumField, EnumFieldViewModel>().ReverseMap();

            CreateMap<AddFieldModel, UpsertFieldViewModel>().ReverseMap();
            CreateMap<EditFieldModel, UpsertFieldViewModel>().ReverseMap();

            CreateMap<AddEnumFieldModel, UpsertEnumFieldViewModel>().ReverseMap();
            CreateMap<EditEnumFieldModel, UpsertEnumFieldViewModel>().ReverseMap();
        }

    }
}
