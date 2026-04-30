using CondosmartWeb.Models;

namespace CondosmartWeb.Services
{
    public interface IMoradorDashboardService
    {
        MoradorDashboardViewModel? Build(string email);
    }
}
