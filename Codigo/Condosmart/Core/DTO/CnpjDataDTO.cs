namespace Core.DTO
{
    /// <summary>
    /// DTO com dados de CNPJ obtidos de API externa
    /// </summary>
    public class CnpjDataDTO
    {
        public string? Cnpj { get; set; }
        public string? Nome { get; set; }
        public string? Rua { get; set; }
        public string? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Cep { get; set; }
        public string? Complemento { get; set; }
        public bool ValorValido { get; set; }
    }
}
