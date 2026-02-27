using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class CepController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;

        public CepController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        // GET /Cep/GetEndereco?cep=01001000
        [HttpGet]
        public async Task<IActionResult> GetEndereco(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return BadRequest("CEP é obrigatório");

            var digits = new string(cep.Where(char.IsDigit).ToArray());
            if (digits.Length != 8)
                return BadRequest("CEP deve conter 8 dígitos");

            var client = _httpFactory.CreateClient();
            var url = $"https://viacep.com.br/ws/{digits}/json/";
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(url);
            }
            catch
            {
                return StatusCode(503, "Serviço de CEP indisponível");
            }

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            if (root.TryGetProperty("erro", out var err) && err.GetBoolean())
                return NotFound();

            string? logradouro = root.TryGetProperty("logradouro", out var p) ? p.GetString() : null;
            string? complemento = root.TryGetProperty("complemento", out var p2) ? p2.GetString() : null;
            string? bairro = root.TryGetProperty("bairro", out var p3) ? p3.GetString() : null;
            string? localidade = root.TryGetProperty("localidade", out var p4) ? p4.GetString() : null;
            string? uf = root.TryGetProperty("uf", out var p5) ? p5.GetString() : null;

            var result = new
            {
                rua = logradouro,
                complemento = complemento,
                bairro = bairro,
                cidade = localidade,
                uf = uf,
                cep = digits
            };

            return Json(result);
        }
    }
}
