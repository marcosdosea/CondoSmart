using AutoMapper;
using CondosmartAPI.Models;
using Core.Models;

namespace CondosmartAPI.Mappers;

public class MoradorProfile : Profile
{
    public MoradorProfile()
    {
        CreateMap<Morador, MoradorViewModel>().ReverseMap();
    }
}
