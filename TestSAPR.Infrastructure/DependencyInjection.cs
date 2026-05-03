using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TestSAPR.Domain.Interfaces.Repositories;
using TestSAPR.Infrastructure.ApplicationDbContext;
using TestSAPR.Infrastructure.Repositories;

namespace TestSAPR.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPartRepository, PartRepository>();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));

            return services;
        }
    }
}
