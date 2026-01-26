using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class CondominioProfile : Profile
    {
        public CondominioProfile()
        {
            CreateMap<Condominio, CondominioViewModel>().ReverseMap();
        }
    }
}
