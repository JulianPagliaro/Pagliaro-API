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
        public async Task<IActionResult> Editar(int? id, [FromBody] GeneroRequestDto generoRequestDto)
        {
            if (!id.HasValue)
                return BadRequest("El ID no puede ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var generoBack = _genero.GetById(id.Value);
            if (generoBack == null)
                return NotFound();

            generoBack = _mapper.Map<Genero>(generoRequestDto);
            _genero.Save(generoBack);
            return Ok();
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