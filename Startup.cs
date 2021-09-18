using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddControllers().AddJsonOptions(options =>
               options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddScoped<ITaskRepository, EFTaskRepository>();
 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskApp", Version = "v1" });
            });

            services.AddDbContext<TaskDbContext>(options => options.UseInMemoryDatabase("tasksDb"));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var error = new Dictionary<string, dynamic>();
                    var message = new Dictionary<string, string>();
                    error.Add("status", "error");
                    foreach (var key in actionContext.ModelState.Keys)
                    {
                        foreach (var parameter in actionContext.ActionDescriptor.Parameters)
                        {
                            var prop = parameter.ParameterType.GetProperty(key);

                            if (prop != null)
                            {
                                var att = prop.GetCustomAttributes(typeof(ValidationAttribute), false).FirstOrDefault() as ValidationAttribute;
                                if (att is RequiredAttribute requiredAttribute)
                                {
                                    message.Add(prop.Name, "Поле является обязательным для заполнения");
                                }
                                if (att is EmailAddressAttribute emailAddressAttribute)
                                {
                                    message.Add(prop.Name, "Неверный email");
                                }
                            }
                        }
                    }
                    error.Add("message", message);
                    return new BadRequestObjectResult(error);
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskApp v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedData.Initialize(app.ApplicationServices.CreateScope().ServiceProvider);
        }
    }
}
