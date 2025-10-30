using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Estudio;
using GameStore.Application.Dtos.Genero;
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
        public async Task<IActionResult> Editar(int? Id, EstudioRequestDto estudioRequestDto)
        {
            if (!Id.HasValue)
            {
                return BadRequest("ID requerido");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Estudio estudioBack = _estudioApp.GetById(Id.Value);

            if (estudioBack is null)
            {
                return NotFound($"No se encontró el estudio con ID {Id.Value}");
            }

            _mapper.Map(estudioRequestDto, estudioBack);

            estudioBack.Id = Id.Value;

            _estudioApp.Save(estudioBack);

            var estudioResponseDto = _mapper.Map<EstudioResponseDto>(estudioBack);

            return Ok(estudioResponseDto);
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
