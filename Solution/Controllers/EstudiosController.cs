using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Estudio;
using GameStore.Entities;
using GameStore.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiosController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EstudiosController> _logger;
        private readonly IApplication<Estudio> _estudio;
        private readonly IMapper _mapper;

        public EstudiosController(
            ILogger<EstudiosController> logger,
            UserManager<User> userManager,
            IApplication<Estudio> estudio,
            IMapper mapper)
        {
            _logger = logger;
            _estudio = estudio;
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
                return Ok(_mapper.Map<IList<EstudioResponseDto>>(_estudio.GetAll()));
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("ById")]
        [Authorize(Roles = "Admin, Client")]

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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