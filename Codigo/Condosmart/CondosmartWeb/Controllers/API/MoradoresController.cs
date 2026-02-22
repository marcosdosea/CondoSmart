using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoradoresController : ControllerBase
    {
        private readonly IMoradorService _service;
        private readonly IMapper _mapper;

        public MoradoresController(IMoradorService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/moradores
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MoradorViewModel>> GetAll()
        {
            var moradores = _service.GetAll();
            var vms = _mapper.Map<List<MoradorViewModel>>(moradores);
            return Ok(vms);
        }

        // GET: api/moradores/{id}
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

        // POST: api/moradores
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MoradorViewModel> Create(MoradorViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var morador = _mapper.Map<Morador>(vm);
            var id = _service.Create(morador);
            vm.Id = id;

            return CreatedAtAction(nameof(GetById), new { id }, vm);
        }

        // PUT: api/moradores/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MoradorViewModel> Update(int id, MoradorViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _service.Edit(_mapper.Map<Morador>(vm));
            return Ok(vm);
        }

        // DELETE: api/moradores/{id}
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
}
