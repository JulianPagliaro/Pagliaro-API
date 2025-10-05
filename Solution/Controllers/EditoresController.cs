using GameStore.Application;
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
        public EditoresController(ILogger<EditoresController> logger, IApplication<Editor> editor)
        {
            _logger = logger;
            _editor = editor;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_editor.GetAll());
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
            return Ok(editor);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Editor editor)
        {
            if (!ModelState.IsValid)
            { return BadRequest(); }
            _editor.Save(editor);
            return Ok(editor.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, Editor editor)
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
            editorBack.Nombre = editor.Nombre;
            editorBack.Pais = editor.Pais;
            editorBack.FechaFundacion = editor.FechaFundacion;
            _editor.Save(editorBack);
            return Ok(editorBack);
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