using ApiRefactor.Models;

namespace ApiRefactor.Repositories.Interfaces
{
    public interface IWaveRepository : IRepositoryBase<Wave>
    {
        Task<List<Wave>> GetAllAsync();
        Task<Wave?> GetByIdAsync(Guid id);
        Task SaveAsync(Wave wave);
        Task UpdateAsync(Wave wave);
    }
}
