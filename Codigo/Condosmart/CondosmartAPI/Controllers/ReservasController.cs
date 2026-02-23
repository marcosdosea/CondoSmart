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
public class ReservasController : ControllerBase
{
    private readonly IReservaService _service;
    private readonly IMapper _mapper;

    public ReservasController(IReservaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todas as reservas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ReservaViewModel>> GetAll()
    {
        var reservas = _service.GetAll();
        return Ok(_mapper.Map<List<ReservaViewModel>>(reservas));
    }

    /// <summary>
    /// Retorna uma reserva pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ReservaViewModel> GetById(int id)
    {
        var reserva = _service.GetById(id);
        if (reserva == null)
            return NotFound();

        return Ok(_mapper.Map<ReservaViewModel>(reserva));
    }

    /// <summary>
    /// Cria uma nova reserva.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ReservaViewModel> Create(ReservaViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var entity = _mapper.Map<Reserva>(vm);
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
    /// Atualiza os dados de uma reserva existente.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ReservaViewModel> Edit(int id, ReservaViewModel vm)
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
            _service.Edit(_mapper.Map<Reserva>(vm));
            return Ok(vm);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Remove uma reserva pelo ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var reserva = _service.GetById(id);
        if (reserva == null)
            return NotFound();

        _service.Delete(id);
        return Ok();
    }
}
