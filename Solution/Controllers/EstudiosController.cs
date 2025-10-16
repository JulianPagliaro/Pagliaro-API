using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Estudio;
using GameStore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiosController : ControllerBase
    {
        private readonly ILogger<EstudiosController> _logger;
        private readonly IApplication<Estudio> _estudioApp;
        private readonly IMapper _mapper;

        public EstudiosController(
            ILogger<EstudiosController> logger,
            IApplication<Estudio> estudioApp,
            IMapper mapper)
        {
            _logger = logger;
            _estudioApp = estudioApp;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            var estudios = _estudioApp.GetAll();
            var response = _mapper.Map<IList<EstudioResponseDto>>(estudios);
            return Ok(response);
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var estudio = _estudioApp.GetById(id.Value);
            if (estudio == null)
                return NotFound();

            var response = _mapper.Map<EstudioResponseDto>(estudio);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] EstudioRequestDto estudioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var estudio = _mapper.Map<Estudio>(estudioDto);
            _estudioApp.Save(estudio);
            return Ok(estudio.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? id, [FromBody] EstudioRequestDto estudioDto)
        {
            if (!id.HasValue)
                return BadRequest("El ID no puede ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var estudioBack = _estudioApp.GetById(id.Value);
            if (estudioBack == null)
                return NotFound();

            estudioBack = _mapper.Map<Estudio>(estudioDto);
            _estudioApp.Save(estudioBack);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var estudioBack = _estudioApp.GetById(id.Value);
            if (estudioBack == null)
                return NotFound();

            _estudioApp.Delete(estudioBack.Id);
            return Ok();
        }
    }
}
