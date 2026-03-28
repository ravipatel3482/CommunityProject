using LogicLevel.DefinationRepository;
using LogicLevel.ImplementationRepository;
using Microsoft.Extensions.DependencyInjection;
namespace LogicLevel
{
    public static class ServicesCollectionExtnsion
    {
        public static IServiceCollection AddLogicLevelLibrary(this IServiceCollection services)
        {
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IIndiaRepository, IndiaRepository>();
            services.AddScoped<IElectronics, Electronics>();
            services.AddScoped<IUnitofWork, UnitofWork>();
            return services;
        }
    }
}
