using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Profiles
{
    public class ReservaProfile : Profile
    {
        public ReservaProfile()
        {
            CreateMap<Reserva, ReservaViewModel>().ReverseMap();
        }
    }
}