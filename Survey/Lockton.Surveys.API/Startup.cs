//using Lockton.Surveys.Services.Proxies;
using Lockton.Surveys.DataAccess.DBModels.Entities;
using Lockton.Surveys.Domain.Model;
using Lockton.Surveys.Web.Behavior;
using Lockton.Surveys.Web.Configuration;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lockton.Surveys.API
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
            services.AddMvc()
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.InvalidModelStateResponseFactory = context =>
                        {
                            var problems = new CustomBadRequest(context);

                            return new BadRequestObjectResult(problems);
                        };
                    });

            services.AddControllers();
            services.addDataBaseService(Configuration);
            services.addAuthenticationService(Configuration);
            services.addSwaggerService(Configuration);

            //services.AddRefitClient<ILocktonFilesProxyService>().ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration.GetSection("Apis:Url").Value));
            //services.AddRefitClient<ILocktonMailProxyService>().ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration.GetSection("Apis:Mail").Value));

            services.addServices();
            services.addAutoMapperService();
            //services.addEmailService(Configuration);
            // services.addConfigurations(Configuration);
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            var mapConfig = new TypeAdapterConfig();

            mapConfig.NewConfig<Survey, ConfigSurvey>()
            .Map(dest => dest.Sections, src => src.Sections);

            mapConfig.NewConfig<Question, ConfigQuestion>()
            .Map(dest => dest, src => src);

            mapConfig.NewConfig<Answer, ConfigAnswer>()
            .Map(dest => dest, src => src);



            services.AddSingleton(mapConfig);
            services.AddScoped<MapsterMapper.IMapper, MapsterMapper.ServiceMapper>();

            var mappingProfileAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name.StartsWith("Lockton.Surveys"));
            if (mappingProfileAssembly != null)
                TypeAdapterConfig.GlobalSettings.Scan(mappingProfileAssembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.Use(async (context, next) =>
            {
                //context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.useSwaggerService(env);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
