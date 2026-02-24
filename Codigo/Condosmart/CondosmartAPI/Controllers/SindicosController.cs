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
public class SindicosController : ControllerBase
{
    private readonly ISindicoService _service;
    private readonly IMapper _mapper;

    public SindicosController(ISindicoService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todos os síndicos cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<SindicoViewModel>> GetAll()
    {
        var sindicos = _service.GetAll();
        return Ok(_mapper.Map<List<SindicoViewModel>>(sindicos));
    }

    /// <summary>
    /// Retorna um síndico pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<SindicoViewModel> GetById(int id)
    {
        var sindico = _service.GetById(id);
        if (sindico == null)
            return NotFound();

        return Ok(_mapper.Map<SindicoViewModel>(sindico));
    }

    /// <summary>
    /// Cria um novo síndico.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SindicoViewModel> Create(SindicoViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var entity = _mapper.Map<Sindico>(vm);
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
    /// Atualiza os dados de um síndico existente.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<SindicoViewModel> Edit(int id, SindicoViewModel vm)
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
            _service.Edit(_mapper.Map<Sindico>(vm));
            return Ok(vm);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Deleta um síndico.
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
