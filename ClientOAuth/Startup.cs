using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

                    config.SaveTokens = true;

                    config.Events = new OAuthEvents()
                    {
                        OnCreatingTicket = context =>
                        {
                            var accessToken = context.AccessToken;
                            var base64payload = accessToken.Split('.')[1];
                            var bytes = Convert.FromBase64String(base64payload);
                            var jsonPayload = Encoding.UTF8.GetString(bytes);
                            var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);

                            foreach (var claim in claims)
                            {
                                context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                            }

                            return Task.CompletedTask;
                        }
                    };
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
