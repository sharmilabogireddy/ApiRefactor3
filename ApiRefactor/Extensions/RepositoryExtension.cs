using ApiRefactor.Data.Contexts;
using ApiRefactor.Repositories;
using ApiRefactor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiRefactor.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection ConfigureRepositoryManager(this IServiceCollection services, IConfiguration configuration)
        {
            //  Create the repository manager registration in the DI.
            services.AddDbContext<WaveRepositoryContext>(options =>
                    options.UseSqlite(configuration.GetValue<string>("Database:ConnectionString")));

            services.AddScoped<IWaveRepository, WaveRepository>();

            return services;
        }
    }
}
