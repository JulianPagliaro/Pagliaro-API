using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Pais;
using GameStore.Entities;
using GameStore.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GameStore.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PaisesController> _logger;
        private readonly IApplication<Pais> _paisApp;
        private readonly IMapper _mapper;

        public PaisesController(ILogger<PaisesController> logger, UserManager<User> userManager, IApplication<Pais> paisApp, IMapper mapper)
        {
            _logger = logger;
            _paisApp = paisApp;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("All")]
        [Authorize(Roles = "Admin, Client")]
        public async Task<IActionResult> All()
        {
            try
            {
                var id = User.FindFirst("Id")?.Value;
                if (string.IsNullOrEmpty(id))
                    return Unauthorized(new { message = "Credenciales inválidas" });

                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return Unauthorized(new { message = "Usuario no encontrado" });

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    var list = _paisApp.GetAll();
                    return Ok(_mapper.Map<IList<PaisResponseDto>>(list));
                }

                return Unauthorized(new { message = "No tiene permisos para ver esta información" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en All Paises");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en All Paises");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en All Paises");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpGet]
        [Route("ById")]
        [Authorize(Roles = "Admin, Client")]
        public async Task<IActionResult> ById(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest(new { message = "Id es requerido" });

                var pais = _paisApp.GetById(Id.Value);
                if (pais is null)
                    return NotFound(new { message = $"País con id {Id.Value} no encontrado" });

                return Ok(_mapper.Map<PaisResponseDto>(pais));
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en ById Paises");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en ById Paises");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en ById Paises");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear(PaisRequestDto paisRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var pais = _mapper.Map<Pais>(paisRequestDto);
                _paisApp.Save(pais);
                return Ok(new { id = pais.Id });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Crear Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Crear Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Crear Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editar(int? Id, PaisRequestDto paisRequestDto)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest(new { message = "ID requerido" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var paisBack = _paisApp.GetById(Id.Value);
                if (paisBack is null)
                    return NotFound(new { message = $"No se encontró el país con ID {Id.Value}" });

                _mapper.Map(paisRequestDto, paisBack);
                paisBack.Id = Id.Value;
                _paisApp.Save(paisBack);

                var paisResponseDto = _mapper.Map<PaisResponseDto>(paisBack);
                return Ok(paisResponseDto);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Editar Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Editar Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Editar Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Borrar(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest(new { message = "Id es requerido" });

                var paisBack = _paisApp.GetById(Id.Value);
                if (paisBack is null)
                    return NotFound(new { message = $"País con id {Id.Value} no encontrado" });

                _paisApp.Delete(paisBack.Id);
                return Ok(new { message = "Eliminado correctamente" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Borrar Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Borrar Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Borrar Pais");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }
    }
}