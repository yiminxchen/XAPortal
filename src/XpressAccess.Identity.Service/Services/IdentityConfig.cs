using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace XpressAccess.Identity.Service
{
    public class IdentityConfig
    {
        // configur openid identity data
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // define the XA Portal Web API resources
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                // for web portal
                new ApiResource ("XAPortal", "XA Management Portal using Asp.net MVC")
                {
                    ApiSecrets = { new Secret("XAPortalSecret".Sha256()) },
                    UserClaims = { "role" },
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "portalscope",
                            DisplayName = "Access XA Portal"
                        }
                    }
                },
                // for web data
                new ApiResource ("XAWebData", "XA Web Data using AngularJS")
                {
                    ApiSecrets = { new Secret("XAWebDataSecret".Sha256()) },
                    UserClaims = { "role" },
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "webdatascope",
                            DisplayName = "Access XA Web Data"
                        }
                    }
                },
            };
        }

        // define the client that can access the resources
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // AngularJS
                new Client
                {
                    ClientId = "angularPortal",
                    ClientName = "Portal access using Angular",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenType = AccessTokenType.Reference,
                    RequireConsent = false,
                    RedirectUris = { "http://localhost:5004/login" },
                    LogoutUri = "http://localhost:5002/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5004" },
                    AllowedCorsOrigins = { "http://localhost:5004" },
                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "webdatascope"
                    }
                },
                // Asp.Net Core MVC
                new Client
                {
                    ClientId = "mvcPortal",
                    ClientName = "Access Portal using MVC",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("mvcSecret".Sha256())
                    },
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    LogoutUri = "http://localhost:5002/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "portalscope"
                    }
                }
            };
        }
    }
}
