using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace ClaimsRoles
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.RequireAuthenticatedSignIn = false;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, 
                    config =>
                    {
                        config.Cookie.Name = "Grandma.Cookie";
                        config.LoginPath = "/Home/Auth/";
                    });

            services.AddAuthorization(options =>
            {
                //var defaultPolicyBuilder = new AuthorizationPolicyBuilder();
                //var defaultPolicy = defaultPolicyBuilder
                //    .RequireAuthenticatedUser()
                //    .RequireClaim(ClaimTypes.DateOfBirth)
                //    .Build();

                //options.DefaultPolicy = defaultPolicy;

                options.AddPolicy("ReqDoB", policyBuilder =>
                {
                    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                });

                //options.AddPolicy("Roles", policyBuilder => { policyBuilder.RequireRole("Admin");  });
            });

            services.AddControllersWithViews(options => 
            {
                var defaultPolicyBuilder = new AuthorizationPolicyBuilder();
                var defaultPolicy = defaultPolicyBuilder
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // сопоставляет запрос с эндпоинтом
            app.UseRouting();

            // ты кто?
            app.UseAuthentication();

            // а тебе сюда можно?
            app.UseAuthorization();

            // содержит соответсвующую эндпоинту логику, выполняемую в ответ на запрос
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
