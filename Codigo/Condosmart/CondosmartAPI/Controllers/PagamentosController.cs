using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Service;
using Core.DTO;
using System;

namespace CondosmartAPI.Controllers
{
    [Authorize] // Apenas usuários autenticados podem acessar
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentosController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        // Injeção de dependência da  interface
        public PagamentosController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        /// <summary>
        /// Endpoint para liquidar uma mensalidade (Funcionalidade Não-CRUD)
        /// </summary>
        [HttpPost("liquidar")]
        public IActionResult Liquidar([FromBody] LiquidarMensalidadeDTO dto)
        {
            try
            {
                // Chama a regra de negócio 
                _pagamentoService.LiquidarMensalidade(dto);

                return Ok(new { mensagem = "Mensalidade liquidada com sucesso!" });
            }
            catch (ArgumentException ex)
            {
                // Se cair na nossa validação retorna erro 400
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception)
            {
                // Se der um erro de banco de dados, retorna erro 500
                return StatusCode(500, new { erro = "Ocorreu um erro interno ao processar o pagamento." });
            }
        }
    }
}