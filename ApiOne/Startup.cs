using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;

namespace ApiOne
{
    public class Startup
    {
        private const string _apiName = "Swagger API Name";

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(
                    options =>
                    {
                        options.Authority = "https://localhost:44301/"; // IdentityServer4 app instance API 
                        options.Audience = "api1"; // who is this app

                        // fix needed to say IdentityServer4 that it should not validate ///
                        options.TokenValidationParameters.ValidateAudience = false;

                        options.Events = new JwtBearerEvents()
                        {
                            OnAuthenticationFailed = c =>
                            {
                                // do some logging or whatever...

                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = _apiName, Version = "v1" });
                //c.IgnoreObsoleteActions();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", _apiName);
            });
        }
    }
}
