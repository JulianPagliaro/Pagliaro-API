using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Editor;
using GameStore.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditoresController : ControllerBase
    {
        private readonly ILogger<EditoresController> _logger;
        private readonly IApplication<Editor> _editor;
        private readonly IMapper _mapper;
        public EditoresController(ILogger<EditoresController> logger, IApplication<Editor> editor, IMapper mapper)
        {
            _logger = logger;
            _editor = editor;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_mapper.Map<IList<EditorResponseDto>>(_editor.GetAll()));
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
            var editor = _mapper.Map<Editor>(editorRequestDto);
            _editor.Save(editor);
            return Ok(editor.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, EditorRequestDto editorRequestDto)
        {
            if (!Id.HasValue)
            {
                return BadRequest("El ID no puede ser nulo.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Editor editorBack = _editor.GetById(Id.Value);
            if (editorBack is null)
            {
                return NotFound();
            }
            editorBack= _mapper.Map<Editor>(editorRequestDto);
            _editor.Save(editorBack);
            return Ok();
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