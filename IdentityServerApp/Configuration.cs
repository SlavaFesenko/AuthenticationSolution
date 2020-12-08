using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerCore
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1"),
                //new ApiScope("api2"),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("api1"),
                //new ApiResource("api2"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    // app which will be accessing to api (api2)
                    ClientId = "client_id",

                    // if no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets = { new Secret("client_secret".Sha256()) },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                //new Client
                //{
                //    // app which will be accessing to api
                //    ClientId = "client_id_mvc",

                //    // if no interactive user, use the clientid/secret for authentication
                //    AllowedGrantTypes = GrantTypes.Code,

                //    // secret for authentication
                //    ClientSecrets = { new Secret("client_secret_mvc".Sha256()) },

                //    // scopes that client has access to
                //    AllowedScopes = { "api1", "api2" }
                //}
            };
    }
}
