using Microsoft.Extensions.DependencyInjection;
using TennisCourt.Application.Interface;
using TennisCourt.Application.Services;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Interfaces.Services;
using TennisCourt.Domain.Services;
using TennisCourt.Infra.Data.Repositories;
using TennisCourt.Infra.Data.Repositories.Base;

namespace TennisCourt.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            RegisterInfraServices(services);
            RegisterApplicationServices(services);

            return services;
        }

        private static void RegisterInfraServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IReservationService, ReservationService>();
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddScoped<IReservationAppService, ReservationAppService>();
        }
    }
}