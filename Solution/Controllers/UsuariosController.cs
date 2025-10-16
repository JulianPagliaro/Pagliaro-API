using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Usuario;
using GameStore.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ILogger<UsuariosController> _logger;
        private readonly IApplication<Usuario> _usuarioApp;
        private readonly IMapper _mapper;

        public UsuariosController(
            ILogger<UsuariosController> logger,
            IApplication<Usuario> usuarioApp,
            IMapper mapper)
        {
            _logger = logger;
            _usuarioApp = usuarioApp;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var usuarios = _usuarioApp.GetAll();
            var response = _mapper.Map<IList<UsuarioResponseDto>>(usuarios);
            return Ok(response);
        }

        [HttpGet("ById")]
        public async Task<IActionResult> ById(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var usuario = _usuarioApp.GetById(id.Value);
            if (usuario == null)
                return NotFound();

            var response = _mapper.Map<UsuarioResponseDto>(usuario);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entidad = _mapper.Map<Usuario>(dto);
            _usuarioApp.Save(entidad);
            return Ok(entidad.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? id, [FromBody] UsuarioRequestDto dto)
        {
            if (!id.HasValue)
                return BadRequest("El ID no puede ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = _usuarioApp.GetById(id.Value);
            if (existente == null)
                return NotFound();

            existente = _mapper.Map<Usuario>(dto);
            _usuarioApp.Save(existente);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var existente = _usuarioApp.GetById(id.Value);
            if (existente == null)
                return NotFound();

            _usuarioApp.Delete(existente.Id);
            return Ok();
        }
    }
}