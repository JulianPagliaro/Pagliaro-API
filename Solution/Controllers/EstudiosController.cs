using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Estudio;
using GameStore.Application.Dtos.Genero;
using GameStore.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiosController : ControllerBase
    {
        private readonly ILogger<EstudiosController> _logger;
        private readonly IApplication<Estudio> _estudio;
        private readonly IMapper _mapper;

        public EstudiosController(
            ILogger<EstudiosController> logger,
            IApplication<Estudio> estudio,
            IMapper mapper)
        {
            _logger = logger;
            _estudio = estudio;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            var items = _estudio.GetAll();
            return Ok(_mapper.Map<IList<EstudioResponseDto>>(items));
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }

            var entity = _estudio.GetById(Id.Value);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EstudioResponseDto>(entity));
        }

        [HttpPost]
        public async Task<IActionResult> Crear(EstudioRequestDto estudioRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entity = _mapper.Map<Estudio>(estudioRequestDto);
            _estudio.Save(entity);
            return Ok(entity.Id);
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

            Estudio estudioBack = _estudio.GetById(Id.Value);

            if (estudioBack is null)
            {
                return NotFound($"No se encontró el estudio con ID {Id.Value}");
            }

            _mapper.Map(estudioRequestDto, estudioBack);

            estudioBack.Id = Id.Value;

            _estudio.Save(estudioBack);

            var discograficaResponseDto = _mapper.Map<EstudioResponseDto>(estudioBack);

            return Ok(discograficaResponseDto);
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }

            var existing = _estudio.GetById(Id.Value);
            if (existing is null)
            {
                return NotFound();
            }

            _estudio.Delete(existing.Id);
            return Ok();
        }
    }
}