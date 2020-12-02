using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClientOAuth
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                // check the cookies to validate that we're authenticated
                options.DefaultAuthenticateScheme = "ClientCookies";
                // when we sign in we deal out cookies
                options.DefaultSignInScheme = "ClientCookies";
                // use this to check if we're allowed to do smth
                options.DefaultChallengeScheme = "OurServer";
            })
                .AddCookie("ClientCookies")
                .AddOAuth("OurServer", config =>
                {
                    config.ClientId = "client_id";         // in real app we will grab and store this values from
                    config.ClientSecret = "client_secret"; // f.e. Google, but now they're mocked

                    config.CallbackPath = "/oauth/callback"; // endpoint for the servers' reply (proceeds automatically)

                    config.AuthorizationEndpoint = "https://localhost:44370/oauth/authorize"; // server auth endpoint
                    config.TokenEndpoint = "https://localhost:44370/oauth/token"; // server token endpoint


                });

            services.AddHttpClient();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
