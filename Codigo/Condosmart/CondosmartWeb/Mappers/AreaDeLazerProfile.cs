using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Profiles
{
    public class AreaDeLazerProfile : Profile
    {
        public AreaDeLazerProfile()
        {
            CreateMap<AreaDeLazer, AreaDeLazerViewModel>().ReverseMap();
        }
    }
}
