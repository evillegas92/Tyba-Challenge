using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Tyba.App.Api.Middleware;
using Tyba.App.Business.Interfaces.Services;
using Tyba.App.Business.MappingProfiles;
using Tyba.App.Business.Services;
using Tyba.App.Persistence.Brokers;
using Tyba.App.Persistence.Contexts;
using Tyba.App.Persistence.Interfaces.Brokers;

namespace Tyba.App.Api
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
            services.AddControllers();

            services.AddDbContext<TybaDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:TybaConnectionString"]));

            //AutoMapper
            var mappingConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TybaMappingProfile>();
            });
            services.AddSingleton(mappingConfiguration);
            services.AddSingleton<IMapper>(new Mapper(mappingConfiguration));

            // my services
            services.AddTransient<IUserBroker, UserBroker>();
            services.AddTransient<IUserService, UserService>();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tyba API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tyba API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
