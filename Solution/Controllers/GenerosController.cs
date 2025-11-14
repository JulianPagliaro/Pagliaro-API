using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Genero;
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
                    var list = _genero.GetAll();
                    return Ok(_mapper.Map<IList<GeneroResponseDto>>(list));
                }

                return Unauthorized(new { message = "No tiene permisos para ver esta información" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en All Generos");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en All Generos");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en All Generos");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpGet]
        [Route("ById")]
        [Authorize(Roles = "Admin, Client")]
        public async Task<IActionResult> ById(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return BadRequest(new { message = "Id es requerido" });

                var genero = _genero.GetById(id.Value);
                if (genero == null)
                    return NotFound(new { message = $"Género con id {id.Value} no encontrado" });

                var response = _mapper.Map<GeneroResponseDto>(genero);
                return Ok(response);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en ById Generos");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en ById Generos");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en ById Generos");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear([FromBody] GeneroRequestDto generoRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var genero = _mapper.Map<Genero>(generoRequestDto);
                _genero.Save(genero);
                return Ok(new { id = genero.Id });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Crear Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Crear Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Crear Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editar(int? Id, GeneroRequestDto generoRequestDto)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest(new { message = "ID requerido" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Genero generoBack = _genero.GetById(Id.Value);
                if (generoBack is null)
                    return NotFound(new { message = $"No se encontró el género con ID {Id.Value}" });

                _mapper.Map(generoRequestDto, generoBack);
                generoBack.Id = Id.Value;
                _genero.Save(generoBack);

                var generoResponseDto = _mapper.Map<GeneroResponseDto>(generoBack);
                return Ok(generoResponseDto);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Editar Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Editar Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Editar Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Borrar(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return BadRequest(new { message = "Id es requerido" });

                var generoBack = _genero.GetById(id.Value);
                if (generoBack == null)
                    return NotFound(new { message = $"Género con id {id.Value} no encontrado" });

                _genero.Delete(generoBack.Id);
                return Ok(new { message = "Eliminado correctamente" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Borrar Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Borrar Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Borrar Genero");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }
    }
}