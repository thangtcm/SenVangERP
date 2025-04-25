using Application.Common.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public static class PersistenceDependencies
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
              b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
              .EnableSensitiveDataLogging()
              .LogTo(Console.WriteLine, LogLevel.Information)
            );

            #region Register Service and UniOfWork
            services.AddScoped<IUnitOfWork<ApplicationDbContext>, ApplicationUnitOfWork>();
            services.AddScoped<ApplicationUnitOfWork>();
            #endregion
            return services;
        }
    }
}
