using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Editor;
using GameStore.Application.Dtos.Genero;
using GameStore.Application.Dtos.Plataforma;
using GameStore.Entities;
using GameStore.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlataformasController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PlataformasController> _logger;
        private readonly IApplication<Plataforma> _plataformaApp;
        private readonly IMapper _mapper;

        public PlataformasController(
            ILogger<PlataformasController> logger,
            UserManager<User> userManager,
            IApplication<Plataforma> plataformaApp,
            IMapper mapper)
        {
            _logger = logger;
            _plataformaApp = plataformaApp;
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
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                var name = User.FindFirst("name");
                var a = User.Claims;
                return Ok(_mapper.Map<IList<PlataformaResponseDto>>(_plataformaApp.GetAll()));
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

            var plataforma = _plataformaApp.GetById(id.Value);
            if (plataforma == null)
                return NotFound();

            var response = _mapper.Map<PlataformaResponseDto>(plataforma);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Crear([FromBody] PlataformaRequestDto plataformaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plataforma = _mapper.Map<Plataforma>(plataformaDto);
            _plataformaApp.Save(plataforma);
            return Ok(plataforma.Id);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Editar(int? Id, PlataformaRequestDto plataformaRequestDto)
        {
            if (!Id.HasValue)
            {
                return BadRequest("ID requerido");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Plataforma plataformaBack = _plataformaApp.GetById(Id.Value);

            if (plataformaBack is null)
            {
                return NotFound($"No se encontró la plataforma con ID {Id.Value}");
            }

            _mapper.Map(plataformaRequestDto, plataformaBack);

            plataformaBack.Id = Id.Value;

            _plataformaApp.Save(plataformaBack);

            var plataformaResponseDto = _mapper.Map<GeneroResponseDto>(plataformaBack);

            return Ok(plataformaResponseDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Borrar(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var plataformaBack = _plataformaApp.GetById(id.Value);
            if (plataformaBack == null)
                return NotFound();

            _plataformaApp.Delete(plataformaBack.Id);
            return Ok();
        }
    }
}
