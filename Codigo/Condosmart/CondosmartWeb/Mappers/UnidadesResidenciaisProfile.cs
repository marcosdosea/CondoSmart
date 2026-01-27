using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class UnidadeResidencialProfile : Profile
    {
        public UnidadeResidencialProfile()
        {
            CreateMap<UnidadesResidenciais, UnidadeResidencialViewModel>()
                .ReverseMap();
        }
    }
}
