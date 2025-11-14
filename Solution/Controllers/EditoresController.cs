using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Editor;
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
    public class EditoresController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EditoresController> _logger;
        private readonly IApplication<Editor> _editor;
        private readonly IMapper _mapper;

        public EditoresController(ILogger<EditoresController> logger, UserManager<User> userManager, IApplication<Editor> editor, IMapper mapper)
        {
            _logger = logger;
            _editor = editor;
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
                    var list = _editor.GetAll();
                    return Ok(_mapper.Map<IList<EditorResponseDto>>(list));
                }

                return Unauthorized(new { message = "No tiene permisos para ver esta información" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en All Editores");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en All Editores");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en All Editores");
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

                Editor editor = _editor.GetById(Id.Value);
                if (editor is null)
                    return NotFound(new { message = $"Editor con id {Id.Value} no encontrado" });

                return Ok(_mapper.Map<EditorResponseDto>(editor));
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en ById Editores");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en ById Editores");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en ById Editores");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear(EditorRequestDto editorRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var artista = _mapper.Map<Editor>(editorRequestDto);
                _editor.Save(artista);
                return Ok(new { id = artista.Id });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Crear Editor");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Crear Editor");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Crear Editor");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editar(int? Id, EditorRequestDto artistaRequestDto)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest(new { message = "ID requerido" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Editor editorBack = _editor.GetById(Id.Value);
                if (editorBack is null)
                    return NotFound(new { message = $"No se encontró el editor con ID {Id.Value}" });

                _mapper.Map(artistaRequestDto, editorBack);
                editorBack.Id = Id.Value;
                _editor.Save(editorBack);

                var artistaResponseDto = _mapper.Map<EditorResponseDto>(editorBack);
                return Ok(artistaResponseDto);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Editar Editor");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Editar Editor");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Editar Editor");
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

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Editor editorBack = _editor.GetById(Id.Value);
                if (editorBack is null)
                    return NotFound(new { message = $"Editor con id {Id.Value} no encontrado" });

                _editor.Delete(editorBack.Id);
                return Ok(new { message = "Eliminado correctamente" });
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error en Borrar Editor");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Mapping", message = "Error al mapear datos." });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error en Borrar Editor");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Database", message = "Error en la base de datos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error en Borrar Editor");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Service", message = "Ocurrió un error en el servicio." });
            }
        }
    }
}