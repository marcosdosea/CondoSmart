using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class VisitanteProfile : Profile
    {
        public VisitanteProfile()
        {
            CreateMap<VisitanteViewModel, Visitantes>().ReverseMap();
        }
    }
}