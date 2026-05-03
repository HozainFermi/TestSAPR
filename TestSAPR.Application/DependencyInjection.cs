using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestSAPR.Application.Interfaces;
using TestSAPR.Application.Services;

namespace TestSAPR.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPartService, PartService>();           

            return services;
        }
    }
}
