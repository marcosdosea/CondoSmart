using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class AtaProfile : Profile
    {
        public AtaProfile()
        {
            CreateMap<Ata, AtaViewModel>()
                .ForMember(dest => dest.DataReuniao, opt => opt.MapFrom(src => 
                    src.DataReuniao.HasValue ? src.DataReuniao.Value.ToDateTime(TimeOnly.MinValue) : DateTime.Now));

            CreateMap<AtaViewModel, Ata>()
                .ForMember(dest => dest.DataReuniao, opt => opt.MapFrom(src => 
                    DateOnly.FromDateTime(src.DataReuniao)));
        }
    }
}
