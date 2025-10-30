using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Genero;
using GameStore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ILogger<GenerosController> _logger;
        private readonly IApplication<Genero> _genero;
        private readonly IMapper _mapper;

        public GenerosController(ILogger<GenerosController> logger, IApplication<Genero> genero, IMapper mapper)
        {
            _logger = logger;
            _genero = genero;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            var generos = _genero.GetAll();
            var response = _mapper.Map<IList<GeneroResponseDto>>(generos);
            return Ok(response);
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var genero = _genero.GetById(id.Value);
            if (genero == null)
                return NotFound();

            var response = _mapper.Map<GeneroResponseDto>(genero);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] GeneroRequestDto generoRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var genero = _mapper.Map<Genero>(generoRequestDto);
            _genero.Save(genero);
            return Ok(genero.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, GeneroRequestDto generoRequestDto)
        {
            if (!Id.HasValue)
            {
                return BadRequest("ID requerido");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Genero generoBack = _genero.GetById(Id.Value);

            if (generoBack is null)
            {
                return NotFound($"No se encontró el género con ID {Id.Value}");
            }

            _mapper.Map(generoRequestDto, generoBack);

            generoBack.Id = Id.Value;

            _genero.Save(generoBack);

            var generoResponseDto = _mapper.Map<GeneroResponseDto>(generoBack);

            return Ok(generoResponseDto);
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var generoBack = _genero.GetById(id.Value);
            if (generoBack == null)
                return NotFound();

            _genero.Delete(generoBack.Id);
            return Ok();
        }
    }
}