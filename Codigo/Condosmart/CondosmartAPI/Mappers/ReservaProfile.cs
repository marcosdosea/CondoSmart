using AutoMapper;
using CondosmartAPI.Models;
using Core.Models;

namespace CondosmartAPI.Mappers;

public class ReservaProfile : Profile
{
    public ReservaProfile()
    {
        CreateMap<Reserva, ReservaViewModel>()
            .ForMember(dest => dest.NomeArea, opt => opt.MapFrom(src => src.Area != null ? src.Area.Nome : null))
            .ForMember(dest => dest.NomeMorador, opt => opt.MapFrom(src => src.Morador != null ? src.Morador.Nome : null))
            .ReverseMap();
    }
}
