using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class ChamadoProfile : Profile
    {
        public ChamadoProfile()
        {
            CreateMap<Chamado, ChamadoViewModel>().ReverseMap();
        }
    }
}
