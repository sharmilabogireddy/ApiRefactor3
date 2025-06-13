using ApiRefactor.Models;

namespace ApiRefactor.Repositories.Interfaces
{
    public interface IWaveRepository : IRepositoryBase<Wave>
    {
        Task<List<Wave>> GetAllAsync();
        Task<Wave?> GetByIdAsync(Guid id);
        Task<Wave> SaveAsync(Wave wave);
        Task<Wave> UpdateAsync(Wave wave);
    }
}
