using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Estudio;
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
                    var list = _estudio.GetAll();
                    return Ok(_mapper.Map<IList<EstudioResponseDto>>(list));
                }

                return Unauthorized(new { message = "No tiene permisos para ver esta información" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en All Estudios");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en All Estudios");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en All Estudios");
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

                var entity = _estudio.GetById(Id.Value);
                if (entity is null)
                    return NotFound(new { message = $"Estudio con id {Id.Value} no encontrado" });

                return Ok(_mapper.Map<EstudioResponseDto>(entity));
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en ById Estudios");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en ById Estudios");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en ById Estudios");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear(EstudioRequestDto estudioRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = _mapper.Map<Estudio>(estudioRequestDto);
                _estudio.Save(entity);
                return Ok(new { id = entity.Id });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Crear Estudio");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Crear Estudio");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Crear Estudio");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editar(int? Id, EstudioRequestDto estudioRequestDto)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest(new { message = "ID requerido" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Estudio estudioBack = _estudio.GetById(Id.Value);
                if (estudioBack is null)
                    return NotFound(new { message = $"No se encontró el estudio con ID {Id.Value}" });

                _mapper.Map(estudioRequestDto, estudioBack);
                estudioBack.Id = Id.Value;
                _estudio.Save(estudioBack);

                var estudioResponseDto = _mapper.Map<EstudioResponseDto>(estudioBack);
                return Ok(estudioResponseDto);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Editar Estudio");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Editar Estudio");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Editar Estudio");
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

                var existing = _estudio.GetById(Id.Value);
                if (existing is null)
                    return NotFound(new { message = $"Estudio con id {Id.Value} no encontrado" });

                _estudio.Delete(existing.Id);
                return Ok(new { message = "Eliminado correctamente" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Borrar Estudio");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Borrar Estudio");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Borrar Estudio");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }
    }
}