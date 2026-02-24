using AutoMapper;
using CondosmartAPI.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Sindico,Morador")]
public class CondominiosController : ControllerBase
{
    private readonly ICondominioService _service;
    private readonly IMapper _mapper;

    public CondominiosController(ICondominioService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todos os condomínios cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<CondominioViewModel>> GetAll()
    {
        var condominios = _service.GetAll();
        return Ok(_mapper.Map<List<CondominioViewModel>>(condominios));
    }

    /// <summary>
    /// Retorna um condomínio pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CondominioViewModel> GetById(int id)
    {
        var condominio = _service.GetById(id);
        if (condominio == null)
            return NotFound();

        return Ok(_mapper.Map<CondominioViewModel>(condominio));
    }

    /// <summary>
    /// Cria um novo condomínio.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CondominioViewModel> Create(CondominioViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var entity = _mapper.Map<Condominio>(vm);
            var id = _service.Create(entity);
            vm.Id = id;
            return CreatedAtAction(nameof(GetById), new { id }, vm);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Atualiza os dados de um condomínio existente.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CondominioViewModel> Edit(int id, CondominioViewModel vm)
    {
        if (id != vm.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existente = _service.GetById(id);
        if (existente == null)
            return NotFound();

        try
        {
            _service.Edit(_mapper.Map<Condominio>(vm));
            return Ok(vm);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Deleta um condomínio.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var existente = _service.GetById(id);
        if (existente == null)
            return NotFound();

        _service.Delete(id);
        return NoContent();
    }
}
