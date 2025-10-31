using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Pais;
using GameStore.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly ILogger<PaisesController> _logger;
        private readonly IApplication<Pais> _paisApp;
        private readonly IMapper _mapper;

        public PaisesController(ILogger<PaisesController> logger, IApplication<Pais> paisApp, IMapper mapper)
        {
            _logger = logger;
            _paisApp = paisApp;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            var paises = _paisApp.GetAll();
            return Ok(_mapper.Map<IList<PaisResponseDto>>(paises));
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }

            var pais = _paisApp.GetById(Id.Value);
            if (pais is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PaisResponseDto>(pais));
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PaisRequestDto paisRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var pais = _mapper.Map<Pais>(paisRequestDto);
            _paisApp.Save(pais);
            return Ok(pais.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, PaisRequestDto paisRequestDto)
        {
            if (!Id.HasValue)
            {
                return BadRequest("ID requerido");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paisBack = _paisApp.GetById(Id.Value);
            if (paisBack is null)
            {
                return NotFound($"No se encontró el país con ID {Id.Value}");
            }

            _mapper.Map(paisRequestDto, paisBack);
            paisBack.Id = Id.Value;
            _paisApp.Save(paisBack);

            var paisResponseDto = _mapper.Map<PaisResponseDto>(paisBack);
            return Ok(paisResponseDto);
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }

            var paisBack = _paisApp.GetById(Id.Value);
            if (paisBack is null)
            {
                return NotFound();
            }

            _paisApp.Delete(paisBack.Id);
            return Ok();
        }
    }
}