using AutoMapper;
using CondosmartAPI.Models;
using Core.Models;

namespace CondosmartAPI.Mappers;

public class SindicoProfile : Profile
{
    public SindicoProfile()
    {
        CreateMap<Sindico, SindicoViewModel>().ReverseMap();
    }
}
