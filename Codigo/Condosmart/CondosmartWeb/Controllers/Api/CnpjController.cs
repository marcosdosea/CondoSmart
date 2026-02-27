using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers.Api
{
    /// <summary>
    /// API para consultar dados de CNPJ
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class CnpjController : ControllerBase
    {
        private readonly ICnpjService _cnpjService;

        public CnpjController(ICnpjService cnpjService)
        {
            _cnpjService = cnpjService;
        }

        /// <summary>
        /// Consulta dados de um CNPJ
        /// </summary>
        /// <param name="cnpj">CNPJ com ou sem formatação</param>
        /// <returns>Dados do CNPJ se encontrado</returns>
        [HttpGet("consultar/{cnpj}")]
        public async Task<IActionResult> ConsultarCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return BadRequest(new { erro = "CNPJ não fornecido." });

            try
            {
                var dados = await _cnpjService.ConsultarCnpjAsync(cnpj);

                if (dados == null)
                    return NotFound(new { erro = "CNPJ não encontrado ou inválido." });

                return Ok(dados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao consultar CNPJ.", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Consulta dados de um CNPJ via POST
        /// </summary>
        /// <param name="request">Objeto contendo o CNPJ</param>
        /// <returns>Dados do CNPJ se encontrado</returns>
        [HttpPost("consultar")]
        public async Task<IActionResult> ConsultarCnpjPost([FromBody] ConsultarCnpjRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Cnpj))
                return BadRequest(new { erro = "CNPJ não fornecido." });

            try
            {
                var dados = await _cnpjService.ConsultarCnpjAsync(request.Cnpj);

                if (dados == null)
                    return NotFound(new { erro = "CNPJ não encontrado ou inválido." });

                return Ok(dados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao consultar CNPJ.", detalhes = ex.Message });
            }
        }
    }

    public class ConsultarCnpjRequest
    {
        public string? Cnpj { get; set; }
    }
}
