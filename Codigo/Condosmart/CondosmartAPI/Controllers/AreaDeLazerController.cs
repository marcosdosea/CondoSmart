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
public class AreaDeLazerController : ControllerBase
{
    private readonly IAreaDeLazerService _service;
    private readonly IMapper _mapper;

    public AreaDeLazerController(IAreaDeLazerService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todas as áreas de lazer cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<AreaDeLazerViewModel>> GetAll()
    {
        var areas = _service.GetAll();
        return Ok(_mapper.Map<List<AreaDeLazerViewModel>>(areas));
    }

    /// <summary>
    /// Retorna uma área de lazer pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<AreaDeLazerViewModel> GetById(int id)
    {
        var area = _service.GetById(id);
        if (area == null)
            return NotFound();

        return Ok(_mapper.Map<AreaDeLazerViewModel>(area));
    }

    /// <summary>
    /// Cria uma nova área de lazer.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Sindico")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<AreaDeLazerViewModel> Create(AreaDeLazerViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<AreaDeLazer>(vm);
        var id = _service.Create(entity);
        vm.Id = id;
        return CreatedAtAction(nameof(GetById), new { id }, vm);
    }

    /// <summary>
    /// Atualiza os dados de uma área de lazer existente.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Sindico")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<AreaDeLazerViewModel> Edit(int id, AreaDeLazerViewModel vm)
    {
        if (id != vm.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existente = _service.GetById(id);
        if (existente == null)
            return NotFound();

        _service.Edit(_mapper.Map<AreaDeLazer>(vm));
        return Ok(vm);
    }

    /// <summary>
    /// Remove uma área de lazer pelo ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Sindico")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var area = _service.GetById(id);
        if (area == null)
            return NotFound();

        _service.Delete(id);
        return Ok();
    }
}
