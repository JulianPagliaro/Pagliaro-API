using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Genero;
using GameStore.Entities;
using GameStore.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GenerosController> _logger;
        private readonly IApplication<Genero> _genero;
        private readonly IMapper _mapper;

        public GenerosController(ILogger<GenerosController> logger, UserManager<User> userManager, IApplication<Genero> genero, IMapper mapper)
        {
            _logger = logger;
            _genero = genero;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("All")]
        [Authorize(Roles = "Admin, Client")]

        public async Task<IActionResult> All()
        {
            var id = User.FindFirst("Id").Value.ToString();
            var user = _userManager.FindByIdAsync(id).Result;
            if (_userManager.IsInRoleAsync(user, "Administrador").Result)
            {
                var name = User.FindFirst("name");
                var a = User.Claims;
                return Ok(_mapper.Map<IList<GeneroResponseDto>>(_genero.GetAll()));
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("ById")]
        [Authorize(Roles = "Admin, Client")]

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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Crear([FromBody] GeneroRequestDto generoRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var genero = _mapper.Map<Genero>(generoRequestDto);
            _genero.Save(genero);
            return Ok(genero.Id);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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