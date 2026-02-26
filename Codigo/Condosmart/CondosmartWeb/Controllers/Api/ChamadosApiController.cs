using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Core.ViewModels;
using Core.Service;
using Core.Models;
using System;
using System.Security.Claims;

namespace CondosmartWeb.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Morador,Admin")]
    public class ChamadosApiController : ControllerBase
    {
        private readonly IChamadosService _service; 
        private readonly IMapper _mapper;

        public ChamadosApiController(IChamadosService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] RegistrarChamadoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var chamado = _mapper.Map<Chamado>(model);

                // Requisito: Identificar quem está logado
                chamado.MoradorId = 1;
                chamado.CondominioId = 1;

                _service.RegistrarChamadoMorador(chamado);

                return Ok(new { mensagem = "Chamado registrado via API com sucesso!", data = DateTime.Now });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}