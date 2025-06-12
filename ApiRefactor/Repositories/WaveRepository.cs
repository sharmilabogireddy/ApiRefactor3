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

        public async Task SaveAsync(Wave wave)
        {
            await _repositoryContext.Waves.AddAsync(wave);

            // Save changes to the database
            await _repositoryContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Wave wave)
        {
            _repositoryContext.Waves.Update(wave);
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
