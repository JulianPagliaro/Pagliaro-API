using AutoMapper;
using GameStore.Application;
using GameStore.Application.Dtos.Review;
using GameStore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ILogger<ReviewsController> _logger;
        private readonly IApplication<Review> _reviewApp;
        private readonly IMapper _mapper;

        public ReviewsController(
            ILogger<ReviewsController> logger,
            IApplication<Review> resenaApp,
            IMapper mapper)
        {
            _logger = logger;
            _reviewApp = resenaApp;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var items = _reviewApp.GetAll();
            var response = _mapper.Map<IList<ReviewResponseDto>>(items);
            return Ok(response);
        }

        [HttpGet("ById")]
        public async Task<IActionResult> ById(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var item = _reviewApp.GetById(id.Value);
            if (item == null)
                return NotFound();

            var response = _mapper.Map<ReviewResponseDto>(item);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ReviewRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entidad = _mapper.Map<Review>(dto);
            _reviewApp.Save(entidad);
            return Ok(entidad.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? id, [FromBody] ReviewRequestDto dto)
        {
            if (!id.HasValue)
                return BadRequest("El ID no puede ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = _reviewApp.GetById(id.Value);
            if (existente == null)
                return NotFound();

            existente = _mapper.Map<Review>(dto);
            _reviewApp.Save(existente);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var existente = _reviewApp.GetById(id.Value);
            if (existente == null)
                return NotFound();

            _reviewApp.Delete(existente.Id);
            return Ok();
        }
    }
}
