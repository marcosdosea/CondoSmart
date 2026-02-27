using Core.DTO;

namespace Core.Service
{
    /// <summary>
    /// Interface para consultar dados de CNPJ em API externa
    /// </summary>
    public interface ICnpjService
    {
        /// <summary>
        /// Consulta informações de CNPJ em uma API externa
        /// </summary>
        /// <param name="cnpj">CNPJ com ou sem formatação</param>
        /// <returns>Dados do CNPJ se encontrado; null caso contrário</returns>
        Task<CnpjDataDTO?> ConsultarCnpjAsync(string cnpj);
    }
}
