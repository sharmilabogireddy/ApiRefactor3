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
        public async Task<ActionResult<Wave>> Save([FromBody] CreateWave wave)
        {

            var newWave = new Wave
            {
                Name = wave.Name,
            };

            var result = await _repository.SaveAsync(newWave);

            return Ok(result);

        }

        [HttpPut]
        public async Task<ActionResult<Wave>> Update([FromBody] UpdateWave wave)
        {
           
           var updateWave = new Wave { 
               Id = wave.Id,
               Name = wave.Name,
               WaveDate = DateTime.Now
           };

            await _repository.UpdateAsync(updateWave);

            return Ok(updateWave);
        }
    }
}
