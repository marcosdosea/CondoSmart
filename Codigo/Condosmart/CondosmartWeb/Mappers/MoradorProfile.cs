using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class MoradorProfile : Profile
    {
        public MoradorProfile()
        {
            CreateMap<Morador, MoradorViewModel>().ReverseMap();
        }
    }
}
