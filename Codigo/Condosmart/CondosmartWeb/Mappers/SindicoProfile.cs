using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class SindicoProfile : Profile
    {
        public SindicoProfile()
        {
            CreateMap<Sindico, SindicoViewModel>().ReverseMap();
        }
    }
}
