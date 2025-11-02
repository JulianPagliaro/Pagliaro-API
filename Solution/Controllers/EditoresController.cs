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
        public async Task<IActionResult> All()
        {
            var id = User.FindFirst("Id").Value.ToString();
            var user = _userManager.FindByIdAsync(id).Result;
            if (_userManager.IsInRoleAsync(user, "Administrador").Result)
            {
                var name = User.FindFirst("name");
                var a = User.Claims;
                return Ok(_mapper.Map<IList<EditorResponseDto>>(_editor.GetAll()));
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }
            Editor editor = _editor.GetById(Id.Value);
            if (editor is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EditorResponseDto>(editor));
        }

        [HttpPost]
        public async Task<IActionResult> Crear(EditorRequestDto editorRequestDto)
        {
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var artista = _mapper.Map<Editor>(editorRequestDto);
            _editor.Save(artista);
            return Ok(artista.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, EditorRequestDto artistaRequestDto)
        {
            if (!Id.HasValue)
            {
                return BadRequest("ID requerido");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Editor editorBack = _editor.GetById(Id.Value);

            if (editorBack is null)
            {
                return NotFound($"No se encontró el editor con ID {Id.Value}");
            }

            _mapper.Map(artistaRequestDto, editorBack);

            editorBack.Id = Id.Value;

            _editor.Save(editorBack);

            var artistaResponseDto = _mapper.Map<EditorResponseDto>(editorBack);

            return Ok(artistaResponseDto);
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? Id)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            Editor editorBack = _editor.GetById(Id.Value);
            if (editorBack is null)
            { return NotFound(); }
            _editor.Delete(editorBack.Id);
            return Ok();
        }
    }
}