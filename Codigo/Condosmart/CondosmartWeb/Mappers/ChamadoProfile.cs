using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.ViewModels; 

namespace CondosmartWeb.Mappers
{
    public class ChamadoProfile : Profile
    {
        public ChamadoProfile()
        {
            CreateMap<Chamado, ChamadoViewModel>().ReverseMap();
            CreateMap<RegistrarChamadoViewModel, Chamado>();
        }
    }
}