using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static System.Threading.Tasks.Task;

namespace ApiOne
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", config =>
            //    {
            //        config.Authority = "https://localhost:44301/";
            //        config.Audience = "api1";
            //    });

            

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            ).AddJwtBearer(cfg =>
                {
                    cfg.Authority = "https://localhost:44301/";
                    cfg.Audience = "api1";
                    cfg.TokenValidationParameters.ValidateAudience = false;

                    cfg.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            // do some logging or whatever...

                            return CompletedTask;
                        }

                    };
                    //cfg.RequireHttpsMetadata = false;
                    //cfg.SaveToken = true;

                    //cfg.TokenValidationParameters = new TokenValidationParameters()
                    //{
                    //    ValidAudience = jwtAudience,
                    //    ValidIssuer = jwtIssuer,
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey))
                    //};

                });

            services.AddControllers();
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
        }
    }
}
