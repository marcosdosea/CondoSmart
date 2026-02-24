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
public class AtasController : ControllerBase
{
    private readonly IAtaService _service;
    private readonly IMapper _mapper;

    public AtasController(IAtaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todas as atas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<AtaViewModel>> GetAll()
    {
        var atas = _service.GetAll();
        return Ok(_mapper.Map<List<AtaViewModel>>(atas));
    }

    /// <summary>
    /// Retorna uma ata pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<AtaViewModel> GetById(int id)
    {
        var ata = _service.GetById(id);
        if (ata == null)
            return NotFound();

        return Ok(_mapper.Map<AtaViewModel>(ata));
    }

    /// <summary>
    /// Cria uma nova ata.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<AtaViewModel> Create(AtaViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var entity = _mapper.Map<Ata>(vm);
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
    /// Atualiza os dados de uma ata existente.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<AtaViewModel> Edit(int id, AtaViewModel vm)
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
            _service.Edit(_mapper.Map<Ata>(vm));
            return Ok(vm);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Deleta uma ata.
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
