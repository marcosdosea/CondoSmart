using AutoMapper;
using CondosmartAPI.Models;
using Core.Models;

namespace CondosmartAPI.Mappers;

public class AtaProfile : Profile
{
    public AtaProfile()
    {
        CreateMap<Ata, AtaViewModel>()
            .ForMember(dest => dest.DataReuniao, opt => opt.MapFrom(src => src.DataReuniao.HasValue ? src.DataReuniao.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue))
            .ForMember(dest => dest.NomeCondominio, opt => opt.MapFrom(src => src.Condominio != null ? src.Condominio.Nome : null))
            .ForMember(dest => dest.NomeSindico, opt => opt.MapFrom(src => src.Sindico != null ? src.Sindico.Nome : null));

        CreateMap<AtaViewModel, Ata>()
            .ForMember(dest => dest.DataReuniao, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DataReuniao)))
            .ForMember(dest => dest.Condominio, opt => opt.Ignore())
            .ForMember(dest => dest.Sindico, opt => opt.Ignore());
    }
}
