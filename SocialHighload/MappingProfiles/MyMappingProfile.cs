using AutoMapper;
using OtusSocial.Models;
using OtusSocial.Service.Model.Dto.People;

namespace OtusSocial.MappingProfiles
{
    public class MyMappingProfile: Profile
    {
        public MyMappingProfile()
        {
            CreateMap<SignUpModel, DtoPerson>()
                .ForMember(d => d.Surname, 
                    o => o.MapFrom(s => s.Surname));
            CreateMap<SignUpModel, DtoPerson>()
                .ForMember(d => d.Name, 
                    o => o.MapFrom(s => s.Name));
            CreateMap<SignUpModel, DtoPerson>()
                .ForMember(d => d.Age, 
                    o => o.MapFrom(s => s.Age));
            CreateMap<SignUpModel, DtoPerson>()
                .ForMember(d => d.City, 
                    o => o.MapFrom(s => s.City));
            CreateMap<SignUpModel, DtoPerson>()
                .ForMember(d => d.Gender, 
                    o => o.MapFrom(s => s.Gender));
            CreateMap<SignUpModel, DtoPerson>()
                .ForMember(d => d.Bio, 
                    o => o.MapFrom(s => s.Bio));
        }
    }
}