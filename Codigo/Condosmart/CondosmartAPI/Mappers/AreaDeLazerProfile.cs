using AutoMapper;
using CondosmartAPI.Models;
using Core.Models;

namespace CondosmartAPI.Mappers;

public class AreaDeLazerProfile : Profile
{
    public AreaDeLazerProfile()
    {
        CreateMap<AreaDeLazer, AreaDeLazerViewModel>().ReverseMap();
    }
}
