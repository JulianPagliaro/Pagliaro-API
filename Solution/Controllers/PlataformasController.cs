using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Editor;
using GameStore.Application.Dtos.Genero;
using GameStore.Application.Dtos.Plataforma;
using GameStore.Entities;
using GameStore.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GameStore.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                    var list = _plataformaApp.GetAll();
                    return Ok(_mapper.Map<IList<PlataformaResponseDto>>(list));
                }

                return Unauthorized(new { message = "No tiene permisos para ver esta información" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en All Plataformas");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en All Plataformas");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en All Plataformas");
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

                var plataforma = _plataformaApp.GetById(id.Value);
                if (plataforma == null)
                    return NotFound(new { message = $"Plataforma con id {id.Value} no encontrada" });

                var response = _mapper.Map<PlataformaResponseDto>(plataforma);
                return Ok(response);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en ById Plataformas");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en ById Plataformas");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en ById Plataformas");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear([FromBody] PlataformaRequestDto plataformaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var plataforma = _mapper.Map<Plataforma>(plataformaDto);
                _plataformaApp.Save(plataforma);
                return Ok(new { id = plataforma.Id });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Crear Plataforma");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Crear Plataforma");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Crear Plataforma");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editar(int? Id, PlataformaRequestDto plataformaRequestDto)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest(new { message = "ID requerido" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Plataforma plataformaBack = _plataformaApp.GetById(Id.Value);
                if (plataformaBack is null)
                    return NotFound(new { message = $"No se encontró la plataforma con ID {Id.Value}" });

                _mapper.Map(plataformaRequestDto, plataformaBack);
                plataformaBack.Id = Id.Value;
                _plataformaApp.Save(plataformaBack);

                var plataformaResponseDto = _mapper.Map<PlataformaResponseDto>(plataformaBack);
                return Ok(plataformaResponseDto);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Editar Plataforma");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Editar Plataforma");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Editar Plataforma");
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

                var plataformaBack = _plataformaApp.GetById(id.Value);
                if (plataformaBack == null)
                    return NotFound(new { message = $"Plataforma con id {id.Value} no encontrada" });

                _plataformaApp.Delete(plataformaBack.Id);
                return Ok(new { message = "Eliminado correctamente" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Borrar Plataforma");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Borrar Plataforma");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Borrar Plataforma");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }
    }
}