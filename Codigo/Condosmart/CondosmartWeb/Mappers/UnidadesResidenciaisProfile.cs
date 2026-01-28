using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class UnidadeResidencialProfile : Profile
    {
        public UnidadeResidencialProfile()
        {
            CreateMap<UnidadesResidenciais, UnidadeResidencialViewModel>()
                .ForMember(dest => dest.CondominioNome, opt => opt.MapFrom(src => 
                    src.Condominio != null ? src.Condominio.Nome : null))
                .ForMember(dest => dest.MoradorNome, opt => opt.MapFrom(src => 
                    src.Morador != null ? src.Morador.Nome : null))
                .ForMember(dest => dest.SindicoNome, opt => opt.MapFrom(src => 
                    src.Sindico != null ? src.Sindico.Nome : null));

            CreateMap<UnidadeResidencialViewModel, UnidadesResidenciais>()
                .ForMember(dest => dest.Condominio, opt => opt.Ignore())
                .ForMember(dest => dest.Morador, opt => opt.Ignore())
                .ForMember(dest => dest.Sindico, opt => opt.Ignore())
                .ForMember(dest => dest.Pagamentos, opt => opt.Ignore())
                .ForMember(dest => dest.Visitantes, opt => opt.Ignore())
                .ForMember(dest => dest.FotoRosto, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}

