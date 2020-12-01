using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServerCore;

namespace IdentityServerApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddIdentityServer()
            //    .AddInMemoryApiScopes(apiScopes: Configuration.GetApiScopes())
            //    .AddInMemoryApiResources(apiResources: Configuration.GetApis())
            //    .AddInMemoryClients(clients: Configuration.GetClients())
                //.AddDeveloperSigningCredential();

            services.AddIdentityServer()
                //.AddInMemoryPersistedGrants()
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                //.AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryClients(Configuration.Clients)
                .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
