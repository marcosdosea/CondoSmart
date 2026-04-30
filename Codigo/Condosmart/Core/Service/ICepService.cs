namespace Core.Service
{
    public interface ICepService
    {
        Task<bool> IsValidAsync(string? cep);
    }
}
