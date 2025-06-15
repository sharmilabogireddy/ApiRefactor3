using ApiRefactor.Common;
using ApiRefactor.Data.Contexts;
using ApiRefactor.Models;
using ApiRefactor.Repositories.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApiRefactor.Repositories
{
    public class WaveRepository : RepositoryBase<Wave>, IWaveRepository
    {
        private readonly WaveRepositoryContext _repositoryContext;

        public WaveRepository(WaveRepositoryContext repositoryContext) : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public async Task<List<Wave>> GetAllAsync()
        {
            
            return await _repositoryContext.Waves.ToListAsync();
        }

        public async Task<Wave?> GetByIdAsync(Guid id)
        {
            return await _repositoryContext.Waves.FindAsync(id);
        }

        public async Task<Wave> SaveAsync(Wave wave)
        {
            var waveToAdd = await ValidateRequestData(wave);

            var result = await _repositoryContext.Waves.AddAsync(waveToAdd);

            await _repositoryContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Wave> UpdateAsync(Wave wave)
        {
            var recordToUpdate = await ValidateRequestData(wave);

            var result =_repositoryContext.Waves.Update(recordToUpdate);

            await _repositoryContext.SaveChangesAsync();

            return result.Entity;
        }

        private async Task<Wave> ValidateRequestData(Wave wave)
        {
            if (wave == null || string.IsNullOrEmpty(wave.Name))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(ConstantStrings.NAME_INVALID);
            }

            if (wave.Id == Guid.Empty)
            {
                // Create scenario
                return new Wave
                {
                    Id = Guid.NewGuid(),
                    Name = wave.Name,
                    WaveDate = DateTime.UtcNow
                };
            }

            // Update scenario
            var existing = await _repositoryContext.Waves.FindAsync(wave.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Wave with ID {wave.Id} not found.");
            }

            existing.Name = wave.Name;
            existing.WaveDate = DateTime.UtcNow;

            return existing;
        }
        
    }
}
