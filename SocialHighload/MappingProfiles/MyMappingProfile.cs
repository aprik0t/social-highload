using System;
using AutoMapper;
using SocialHighload.Models;
using SocialHighload.Service.Model.Dto.Person;
using SocialHighload.Service.Model.Enums;

namespace SocialHighload.MappingProfiles
{
    public class MyMappingProfile: Profile
    {
        public MyMappingProfile()
        {
            CreateMap<SignUpModel, DtoPerson>();
//                .ForMember(d => d.Surname, o => o.MapFrom(s => s.Surname))
//                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
//                .ForMember(d => d.Age, o => o.MapFrom(s => s.Age))
//                .ForMember(d => d.City, o => o.MapFrom(s => s.City))
//                .ForMember(d => d.Gender,o => o.MapFrom(s => Enum.Parse<Gender>(s.Gender.ToString())))
//                .ForMember(d => d.Bio, o => o.MapFrom(s => s.Bio));
            CreateMap<DtoPerson, DtoUpdatePerson>();
            CreateMap<DtoUpdatePerson, DtoPerson>();
        }
    }
}