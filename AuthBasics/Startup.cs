using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthBasics
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

            services.AddControllersWithViews();
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
