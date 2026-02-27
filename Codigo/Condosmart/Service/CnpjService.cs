using Core.DTO;
using Core.Service;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Service
{
    /// <summary>
    /// Implementa serviços para consultar dados de CNPJ em API externa
    /// </summary>
    public class CnpjService : ICnpjService
    {
        private readonly HttpClient _httpClient;
        // Preferir BrasilAPI por ser mais estável; fallback para ReceitaWS
        private const string BrasilApiUrl = "https://brasilapi.com.br/api/cnpj/v1";
        private const string ReceitaWsUrl = "https://www.receitaws.com.br/v1/cnpj";

        public CnpjService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Consulta informações de CNPJ em uma API externa
        /// </summary>
        /// <param name="cnpj">CNPJ com ou sem formatação</param>
        /// <returns>Dados do CNPJ se encontrado; null caso contrário</returns>
        public async Task<CnpjDataDTO?> ConsultarCnpjAsync(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return null;

            try
            {
                string cnpjLimpo = Regex.Replace(cnpj, @"\D", "");

                if (cnpjLimpo.Length != 14)
                    return null;
                // 1) Tentar BrasilAPI
                try
                {
                    var resp = await _httpClient.GetAsync($"{BrasilApiUrl}/{cnpjLimpo}");
                    if (resp.IsSuccessStatusCode)
                    {
                        var content = await resp.Content.ReadAsStringAsync();
                        var brasil = System.Text.Json.JsonSerializer.Deserialize<BrasilApiResponse>(content,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (brasil != null && !string.IsNullOrWhiteSpace(brasil.Cnpj))
                        {
                            return new CnpjDataDTO
                            {
                                Cnpj = FormatarCnpj(brasil.Cnpj),
                                Nome = brasil.Nome ?? brasil.Fantasia,
                                Rua = BrasilApiNormalize(brasil.Logradouro),
                                Numero = brasil.Numero,
                                Bairro = brasil.Bairro,
                                Cidade = brasil.Municipio,
                                Uf = brasil.Uf,
                                Cep = FormatarCep(brasil.Cep),
                                Complemento = brasil.Complemento,
                                ValorValido = true
                            };
                        }
                    }
                }
                catch
                {
                    // ignora e tenta fallback
                }

                // 2) Fallback ReceitaWS
                var response = await _httpClient.GetAsync($"{ReceitaWsUrl}/{cnpjLimpo}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var contentWs = await response.Content.ReadAsStringAsync();
                var dados = System.Text.Json.JsonSerializer.Deserialize<ReceitaWsResponse>(contentWs,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (dados == null)
                    return null;

                // ReceitaWS returns status string sometimes; check fields
                return new CnpjDataDTO
                {
                    Cnpj = FormatarCnpj(dados.Cnpj),
                    Nome = dados.Nome,
                    Rua = dados.Logradouro,
                    Numero = dados.Numero,
                    Bairro = dados.Bairro,
                    Cidade = dados.Municipio,
                    Uf = dados.Uf,
                    Cep = FormatarCep(dados.Cep),
                    Complemento = dados.Complemento,
                    ValorValido = true
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string BrasilApiNormalize(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s ?? string.Empty;
            return s;
        }

        private static string FormatarCnpj(string? cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return cnpj ?? string.Empty;

            string limpo = Regex.Replace(cnpj, @"\D", "");
            if (limpo.Length != 14)
                return cnpj;

            return $"{limpo.Substring(0, 2)}.{limpo.Substring(2, 3)}.{limpo.Substring(5, 3)}/{limpo.Substring(8, 4)}-{limpo.Substring(12, 2)}";
        }

        private static string FormatarCep(string? cep)
        {
            if (string.IsNullOrEmpty(cep))
                return cep ?? string.Empty;

            string limpo = Regex.Replace(cep, @"\D", "");
            if (limpo.Length != 8)
                return cep;

            return $"{limpo.Substring(0, 5)}-{limpo.Substring(5, 3)}";
        }

        private class ReceitaWsResponse
        {
            [JsonPropertyName("cnpj")]
            public string? Cnpj { get; set; }

            [JsonPropertyName("nome")]
            public string? Nome { get; set; }

            [JsonPropertyName("logradouro")]
            public string? Logradouro { get; set; }

            [JsonPropertyName("numero")]
            public string? Numero { get; set; }

            [JsonPropertyName("bairro")]
            public string? Bairro { get; set; }

            [JsonPropertyName("municipio")]
            public string? Municipio { get; set; }

            [JsonPropertyName("uf")]
            public string? Uf { get; set; }

            [JsonPropertyName("cep")]
            public string? Cep { get; set; }

            [JsonPropertyName("complemento")]
            public string? Complemento { get; set; }

            [JsonPropertyName("status")]
            public int Status { get; set; }
        }

        private class BrasilApiResponse
        {
            [JsonPropertyName("cnpj")]
            public string? Cnpj { get; set; }

            [JsonPropertyName("nome")]
            public string? Nome { get; set; }

            [JsonPropertyName("fantasia")]
            public string? Fantasia { get; set; }

            [JsonPropertyName("logradouro")]
            public string? Logradouro { get; set; }

            [JsonPropertyName("numero")]
            public string? Numero { get; set; }

            [JsonPropertyName("bairro")]
            public string? Bairro { get; set; }

            [JsonPropertyName("municipio")]
            public string? Municipio { get; set; }

            [JsonPropertyName("uf")]
            public string? Uf { get; set; }

            [JsonPropertyName("cep")]
            public string? Cep { get; set; }

            [JsonPropertyName("complemento")]
            public string? Complemento { get; set; }
        }
    }
}
