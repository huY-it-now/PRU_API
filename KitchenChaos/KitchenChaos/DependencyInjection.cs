﻿using Application.Interfaces;
using System.Diagnostics;
using WebAPI.Services;

namespace WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            services.AddSingleton<Stopwatch>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
