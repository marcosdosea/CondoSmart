using AutoMapper;
using CondosmartAPI.Models;
using Core.Models;

namespace CondosmartAPI.Mappers;

public class CondominioProfile : Profile
{
    public CondominioProfile()
    {
        CreateMap<Condominio, CondominioViewModel>().ReverseMap();
    }
}
