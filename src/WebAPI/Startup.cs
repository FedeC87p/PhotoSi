using AutoMapper;
using Infrastracture.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PhotoSi.AC.Modules;
using PhotoSi.Interfaces.Configuration;
using WebAPI.BackgroundService;
using WebAPI.ModelViews;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IHostEnvironment HostEnvironment { get; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseConfig>(Configuration.GetSection("Database"));
            services.Configure<GeneralConfig>(Configuration.GetSection("General"));

            configureCORS(services);

            ApplicationCore.ConfigureApplicationCore(services);
            InfrastructureModule.ConfiguresInfrastructure(services, Configuration, HostEnvironment.ContentRootPath);

            services.AddHostedService<MigratorDBHostedService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });

            services.AddAutoMapper(typeof(WebAPI.ModelViews.AutoMapperConfiguration), typeof(PhotoSi.AC.AutoMapperConfiguration));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));

            app.UseCors("AllowAll");

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void configureCORS(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(i => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
        }
    }
}
