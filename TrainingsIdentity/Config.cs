// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace StsServerIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
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
            string trainingsPlannerApi = configuration["TrainingsPlannerApiBaseUrl"];
            string identiyServerApi = configuration["TrainingsIdentityApiBaseUrl"];
             return new Client[]
            {
                // SPA client mit implicit flow
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = trainingsPlannerApi,
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
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
                    
                    AllowedScopes = {"openid", "profile", "api1", IdentityServerConstants.LocalApi.ScopeName}
                },
                new Client
                {
                    ClientId = "demo_api_swagger",
                    ClientName = "Swagger UI for demo_api",
                    ClientSecrets = {new Secret(configuration["SwaggerClientSecret"].Sha256())}, // change me!

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {trainingsPlannerApi+"/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {trainingsPlannerApi},
                    AllowedScopes = {"api1"}
                },
                new Client
                {
                    ClientId = "TrainingsIdentiySwagger",
                    ClientName = "Swagger UI for IdentityServer",
                    ClientSecrets = {new Secret(configuration["SwaggerClientSecret"].Sha256())}, // change me!

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {identiyServerApi+"/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {identiyServerApi},
                    AllowedScopes = {IdentityServerConstants.LocalApi.ScopeName}
                }
            };
        }
    }

}
