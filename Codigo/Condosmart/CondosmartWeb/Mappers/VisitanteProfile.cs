using AutoMapper;
using Core.Models;
using CondosmartWeb.Models;

namespace CondosmartWeb.Mappers
{
    public class VisitanteProfile : Profile
    {
        public VisitanteProfile()
        {
            CreateMap<Visitantes, VisitanteViewModel>().ReverseMap();
        }
    }
}