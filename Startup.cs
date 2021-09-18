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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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

        public void ConfigureServices(IServiceCollection services)
        {
            var key = "This is my first Test Key";
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                };
            });

            services.AddControllers().AddJsonOptions(options =>
               options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSingleton<IJwtAuth>(new Auth(key));
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
