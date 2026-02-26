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
                // Ensina o Mapper a pegar o Nome dentro do objeto Area
                .ForMember(dest => dest.NomeArea, opt => opt.MapFrom(src => src.Area.Nome))
                // Ensina o Mapper a pegar o Nome dentro do objeto Morador
                .ForMember(dest => dest.NomeMorador, opt => opt.MapFrom(src => src.Morador.Nome))
                .ReverseMap();
        }
    }
}