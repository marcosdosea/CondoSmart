using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Profiles
{
    public class MensalidadeProfile : Profile
    {
        public MensalidadeProfile()
        {
            CreateMap<Mensalidade, MensalidadeViewModel>()
                .ForMember(dest => dest.CondominioNome, opt => opt.MapFrom(src => src.Condominio != null ? src.Condominio.Nome : null))
                .ForMember(dest => dest.UnidadeIdentificador, opt => opt.MapFrom(src => src.Unidade != null ? src.Unidade.Identificador : null))
                .ForMember(dest => dest.MoradorNome, opt => opt.MapFrom(src => src.Morador != null ? src.Morador.Nome : null))
                .ReverseMap();
        }
    }
}
