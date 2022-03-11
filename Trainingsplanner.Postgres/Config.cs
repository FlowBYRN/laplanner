// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Duende.IdentityServer.Models;
using Duende.IdentityServer;

namespace Trainingsplanner.Postgres
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new Duende.IdentityServer.Models.IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),

                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1", "Api 1"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("api1", "Api 1")
                {
                    Scopes={"api1"}
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };



        public static IEnumerable<Client> Clients(IConfiguration configuration)
        {
            string trainingsPlannerApi = configuration["ApiBaseUrl"];
            return new Client[]
           {
                // SPA client mit implicit flow
                new Client
                {
                    ClientId = "IdentityServerSPA",
                    ClientName = "SPA Client",
                    ClientUri = trainingsPlannerApi,
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =
                    {
                        trainingsPlannerApi+"/index.html",
                        trainingsPlannerApi+"/callback",
                        trainingsPlannerApi+"/silent.html",
                        trainingsPlannerApi+"/popup.html",
                    },

                    PostLogoutRedirectUris = {trainingsPlannerApi+"/index.html"},
                    AllowedCorsOrigins = {trainingsPlannerApi},

                    AllowedScopes = {"openid", "profile", "api1"}
                },
                new Client
                {
                    ClientId = "laplanner_swagger",
                    ClientName = "Swagger UI for laplanner",
                    ClientSecrets = {new Secret(configuration["SwaggerClientSecret"].Sha256())}, // change me!

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {trainingsPlannerApi+"/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {trainingsPlannerApi},
                    AllowedScopes = {"api1"}
                },
           };
        }
    }

}
