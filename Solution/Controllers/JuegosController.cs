using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Juego;
using GameStore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegosController : ControllerBase
    {
        private readonly ILogger<JuegosController> _logger;
        private readonly IApplication<Juego> _juegoApp;
        private readonly IMapper _mapper;

        public JuegosController(
            ILogger<JuegosController> logger,
            IApplication<Juego> juegoApp,
            IMapper mapper)
        {
            _logger = logger;
            _juegoApp = juegoApp;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            var juegos = _juegoApp.GetAll();
            var response = _mapper.Map<IList<JuegoResponseDto>>(juegos);
            return Ok(response);
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var juego = _juegoApp.GetById(id.Value);
            if (juego == null)
                return NotFound();

            var response = _mapper.Map<JuegoResponseDto>(juego);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] JuegoRequestDto juegoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var juego = _mapper.Map<Juego>(juegoDto);
            _juegoApp.Save(juego);
            return Ok(juego.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? id, [FromBody] JuegoRequestDto juegoDto)
        {
            if (!id.HasValue)
                return BadRequest("El ID no puede ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var juegoBack = _juegoApp.GetById(id.Value);
            if (juegoBack == null)
                return NotFound();

            juegoBack = _mapper.Map<Juego>(juegoDto);
            _juegoApp.Save(juegoBack);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var juegoBack = _juegoApp.GetById(id.Value);
            if (juegoBack == null)
                return NotFound();

            _juegoApp.Delete(juegoBack.Id);
            return Ok();
        }
    }
}