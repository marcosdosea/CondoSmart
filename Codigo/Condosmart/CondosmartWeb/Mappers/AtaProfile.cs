using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Mappers
{
    public class AtaProfile : Profile
    {
        public AtaProfile()
        {
            CreateMap<Ata, AtaViewModel>().ReverseMap();
        }
    }
}
