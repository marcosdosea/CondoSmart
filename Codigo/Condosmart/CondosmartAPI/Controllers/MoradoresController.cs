using AutoMapper;
using CondosmartAPI.Data;
using CondosmartAPI.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Sindico")]
public class MoradoresController : ControllerBase
{
    private readonly IMoradorService _service;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public MoradoresController(
        IMoradorService service,
        IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _service = service;
        _mapper = mapper;
        _userManager = userManager;
    }

    /// <summary>
    /// Retorna todos os moradores cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<MoradorViewModel>> GetAll()
    {
        var moradores = _service.GetAll();
        return Ok(_mapper.Map<List<MoradorViewModel>>(moradores));
    }

    /// <summary>
    /// Retorna um morador pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MoradorViewModel> GetById(int id)
    {
        var morador = _service.GetById(id);
        if (morador == null)
            return NotFound();

        return Ok(_mapper.Map<MoradorViewModel>(morador));
    }

    /// <summary>
    /// Cria um novo morador e gera automaticamente um usuário Identity com a role Morador.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MoradorViewModel>> Create(MoradorViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(vm.Email))
        {
            ModelState.AddModelError(nameof(vm.Email), "O e-mail é obrigatório para criação do acesso do morador.");
            return BadRequest(ModelState);
        }

        var emailExistente = await _userManager.FindByEmailAsync(vm.Email);
        if (emailExistente != null)
        {
            ModelState.AddModelError(nameof(vm.Email), "Já existe um usuário cadastrado com este e-mail.");
            return Conflict(ModelState);
        }

        // 1. Persiste o Morador na base de domínio
        var entity = _mapper.Map<Morador>(vm);
        var id = _service.Create(entity);
        vm.Id = id;

        // 2. Cria usuário Identity vinculado ao Morador
        var novoUsuario = new ApplicationUser
        {
            UserName = vm.Email,
            Email = vm.Email,
            NomeCompleto = vm.Nome,
            EmailConfirmed = true
        };

        // Senha temporária padrão — deve ser trocada no primeiro acesso
        const string senhaTemporaria = "Condo@12345!";

        var resultado = await _userManager.CreateAsync(novoUsuario, senhaTemporaria);
        if (!resultado.Succeeded)
        {
            // Rollback compensatório: remove o Morador já salvo
            _service.Delete(id);
            foreach (var erro in resultado.Errors)
                ModelState.AddModelError(string.Empty, erro.Description);
            return BadRequest(ModelState);
        }

        // 3. Atribui a role Morador
        await _userManager.AddToRoleAsync(novoUsuario, "Morador");

        return CreatedAtAction(nameof(GetById), new { id }, vm);
    }

    /// <summary>
    /// Atualiza os dados de um morador existente.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<MoradorViewModel> Edit(int id, MoradorViewModel vm)
    {
        if (id != vm.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _service.Edit(_mapper.Map<Morador>(vm));
        return Ok(vm);
    }

    /// <summary>
    /// Remove um morador pelo ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var morador = _service.GetById(id);
        if (morador == null)
            return NotFound();

        _service.Delete(id);
        return Ok();
    }
}
