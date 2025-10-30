using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Genero;
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
        public async Task<IActionResult> Editar(int? Id, PlataformaRequestDto plataformaRequestDto)
        {
            if (!Id.HasValue)
            {
                return BadRequest("ID requerido");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Plataforma plataformaBack = _plataformaApp.GetById(Id.Value);

            if (plataformaBack is null)
            {
                return NotFound($"No se encontró la plataforma con ID {Id.Value}");
            }

            _mapper.Map(plataformaRequestDto, plataformaBack);

            plataformaBack.Id = Id.Value;

            _plataformaApp.Save(plataformaBack);

            var plataformaResponseDto = _mapper.Map<GeneroResponseDto>(plataformaBack);

            return Ok(plataformaResponseDto);
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
