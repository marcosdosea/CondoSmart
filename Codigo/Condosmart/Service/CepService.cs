using System.Text.Json;
using System.Text.RegularExpressions;
using Core.Service;

namespace Service
{
    public class CepService : ICepService
    {
        private readonly IHttpClientFactory? _httpClientFactory;

        public CepService(IHttpClientFactory? httpClientFactory = null)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> IsValidAsync(string? cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return false;

            var digits = Regex.Replace(cep, @"\D", "");
            if (digits.Length != 8)
                return false;

            if (_httpClientFactory is null)
                return true;

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://viacep.com.br/ws/{digits}/json/");
                if (!response.IsSuccessStatusCode)
                    return false;

                var content = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(content);
                var root = document.RootElement;

                return !root.TryGetProperty("erro", out var erro) || !erro.GetBoolean();
            }
            catch
            {
                return false;
            }
        }
    }
}
