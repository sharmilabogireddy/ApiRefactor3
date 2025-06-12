using ApiRefactor.Models;
using ApiRefactor.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiRefactor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WavesController : ControllerBase
    {
        private readonly IWaveRepository _repository;

        public WavesController(IWaveRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Wave>>> Get()
        {
            var waves = await _repository.GetAllAsync();
            return Ok(waves);
        }

       [HttpGet("{id}")]
        public async Task<ActionResult<Wave>> GetById(Guid id)
        {
            var wave = await _repository.GetByIdAsync(id);
            if (wave == null)
                return NotFound();

            return Ok(wave);
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody] CreateWave wave)
        {
            if (wave == null)
                return BadRequest("Wave object is null.");

            var newWave = new Wave
            {
                Id = Guid.NewGuid(),
                Name = wave.Name,
                WaveDate = DateTime.UtcNow
            };

            await _repository.SaveAsync(newWave);

            return CreatedAtAction(nameof(GetById), new { id = newWave.Id }, newWave);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateWave wave)
        {
            if (wave == null || wave.Id == Guid.Empty)
                return BadRequest("Invalid wave data.");

            var existingWave = await _repository.GetByIdAsync(wave.Id);
            if (existingWave == null)
                return NotFound($"Wave with ID {wave.Id} not found.");

            existingWave.Name = wave.Name;
            //existingWave.WaveDate = wave.WaveDate;

            await _repository.UpdateAsync(existingWave);

            return Ok(existingWave);
        }
    }
}
