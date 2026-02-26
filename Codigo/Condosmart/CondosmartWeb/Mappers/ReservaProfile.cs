using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Profiles
{
    public class ReservaProfile : Profile
    {
        public ReservaProfile()
        {
            CreateMap<Reserva, ReservaViewModel>()
                .ForMember(dest => dest.NomeArea,    opt => opt.MapFrom(src => src.Area != null    ? src.Area.Nome    : null))
                .ForMember(dest => dest.NomeMorador, opt => opt.MapFrom(src => src.Morador != null ? src.Morador.Nome : null));

            CreateMap<ReservaViewModel, Reserva>()
                .ForMember(dest => dest.Area,       opt => opt.Ignore())
                .ForMember(dest => dest.Morador,    opt => opt.Ignore())
                .ForMember(dest => dest.Condominio, opt => opt.Ignore());
        }
    }
}