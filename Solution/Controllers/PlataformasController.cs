using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Plataforma;
using GameStore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlataformasController : ControllerBase
    {
        private readonly ILogger<PlataformasController> _logger;
        private readonly IApplication<Plataforma> _plataformaApp;
        private readonly IMapper _mapper;

        public PlataformasController(
            ILogger<PlataformasController> logger,
            IApplication<Plataforma> plataformaApp,
            IMapper mapper)
        {
            _logger = logger;
            _plataformaApp = plataformaApp;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            var plataformas = _plataformaApp.GetAll();
            var response = _mapper.Map<IList<PlataformaResponseDto>>(plataformas);
            return Ok(response);
        }

        [HttpGet]
        [Route("ById")]
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
        public async Task<IActionResult> Crear([FromBody] PlataformaRequestDto plataformaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plataforma = _mapper.Map<Plataforma>(plataformaDto);
            _plataformaApp.Save(plataforma);
            return Ok(plataforma.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? id, [FromBody] PlataformaRequestDto plataformaDto)
        {
            if (!id.HasValue)
                return BadRequest("El ID no puede ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plataformaBack = _plataformaApp.GetById(id.Value);
            if (plataformaBack == null)
                return NotFound();

            plataformaBack = _mapper.Map<Plataforma>(plataformaDto);
            _plataformaApp.Save(plataformaBack);
            return Ok();
        }

        [HttpDelete]
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
