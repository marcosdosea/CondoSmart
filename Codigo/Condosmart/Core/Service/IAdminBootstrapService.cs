namespace Core.Service
{
    public interface IAdminBootstrapService
    {
        Task<bool> CriarAdminAsync(string nomeCompleto, string email, string senha);
    }
}
