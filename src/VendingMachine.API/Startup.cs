using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using MediatR;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis.Extensions.Core.Configuration;
using VendingMachine.API.Infrastructure.Filters;
using Autofac;
using VendingMachine.API.Infrastructure.AutofacModules;
using Autofac.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using VendingMachine.API.Infrastructure;

namespace VendingMachine.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc();

            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);
            services
                .AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(o => o.MinLevel = LogLevel.Debug);

            services.AddTransient<IStartupFilter, SeedStartupFilter>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //configure auto fac here
            builder.RegisterModule(new MediatorModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

    public static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc(options =>
                    {
                        options.Filters.Add(typeof(GlobalExceptionFilter));
                    })
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    });

            return services;
        }
    }
}
