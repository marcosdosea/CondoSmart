using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Profiles
{
    public class AreaDeLazerProfile : Profile
    {
        public AreaDeLazerProfile()
        {
            CreateMap<AreaDeLazer, AreaDeLazerViewModel>();

            CreateMap<AreaDeLazerViewModel, AreaDeLazer>()
                .ForMember(dest => dest.ImagemNomeOriginal, opt => opt.Ignore())
                .ForMember(dest => dest.ImagemCaminho, opt => opt.Ignore());
        }
    }
}
