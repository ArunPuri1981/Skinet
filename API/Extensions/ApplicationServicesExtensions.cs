using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Error;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<StoreContext>(opt =>
             {
                 opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
             });

            services.AddSingleton<IConnectionMultiplexer>(c => {
                var options=ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            //builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionsContext =>
                {
                    var error = actionsContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = error
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
            
            return services;
        }
    }
}