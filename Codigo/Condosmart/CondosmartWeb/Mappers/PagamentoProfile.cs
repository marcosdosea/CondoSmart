using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;

namespace CondosmartWeb.Profiles
{
    public class PagamentoProfile : Profile
    {
        public PagamentoProfile()
        {
            CreateMap<Pagamento, PagamentoViewModel>().ReverseMap();
        }
    }
}
