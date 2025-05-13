// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EcoRoute.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
                new ApiResource("ResourceDataCollection")
                {
                    Scopes=
                    {
                        "DataCollectionFullPermission",
                        "DataCollectionReadPermission"
                    }
                },
                new ApiResource("ResourceDataProcessing")
                {
                    Scopes =
                    {
                        "DataProcessingFullPermission",
                        "DataProcessingReadPermission",
                    }
                },
                new ApiResource("ResourceRouteOptimization")
                {
                    Scopes =
                    {
                        "RouteOptimizationFullPermission",
                    }
                },
                new ApiResource("ResourceSupports")
                {
                    Scopes =
                    {
                        "SupportsFullPermission",
                        "SupportsReadPermission",
                    }
                },
                new ApiResource("ResourceSettings")
                {
                    Scopes =
                    {
                        "SettingsFullPermission",
                    }
                },
                new ApiResource("ResourceOcelot")
                {
                    Scopes =
                    {
                        "OcelotFullPermission"
                    }
                },
                new ApiResource("ResourceNotifications")
                {
                    Scopes =
                    {
                        "NotificationsFullPermission",
                        "NotificationsReadPermission"
                    }
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
                new ApiScope("DataCollectionFullPermission","Full authority for dc operations"),
                new ApiScope("DataCollectionReadPermission","Read authority for dc operations"),
                new ApiScope("DataProcessingFullPermission","Full authority for dp operations"),
                new ApiScope("DataProcessingReadPermission","Read authority for dp operations"),
                new ApiScope("RouteOptimizationFullPermission","Full authority for ro operations"),
                new ApiScope("SupportsFullPermission","Full authority for support operations"),
                new ApiScope("SupportsReadPermission","Read authority for support operations"),
                new ApiScope("SettingsFullPermission","Full authority for settings operations"),
                new ApiScope("NotificationsFullPermission", "Full authority for notification operations"),
                new ApiScope("NotificationsReadPermission", "Read authority for notification operations"),
                new ApiScope("OcelotFullPermission","Full for oc operations"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<Client> Clients => new Client[]
        { 
                //SuperAdmin
                new Client
                {
                    ClientId = "EcoRouteSuperAdminId",
                    ClientName = "EcoRoute Super Admin",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // Cookie ve JWT kullanacaksan bu olmalı
                    ClientSecrets = { new Secret("ecoroutesecret".Sha256()) },
                    AllowedScopes =
                    {
                        "DataCollectionFullPermission",
                        "DataProcessingFullPermission",
                        "RouteOptimizationFullPermission",
                        "SupportsFullPermission",
                        "SupportsReadPermission",
                        "SettingsFullPermission",
                        "OcelotFullPermission",
                        "NotificationsFullPermission",
                        "NotificationsReadPermission",

                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AccessTokenLifetime = 3600,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },

                //manager
                new Client
                {
                    ClientId="EcoRouteAdminId",
                    ClientName="EcoRoute Admin",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets={new Secret("ecoroutesecret".Sha256())},
                    AllowedScopes={
                        "DataCollectionFullPermission",
                        "DataProcessingFullPermission",
                        "RouteOptimizationFullPermission",
                        "SupportsFullPermission",
                        "NotificationsFullPermission",
                    }
                },

                //Driver
                new Client
                {
                    ClientId="EcoRouteDriverId",
                    ClientName="EcoRoute Driver",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets={new Secret("ecoroutesecret".Sha256())},
                    AllowedScopes={
                        "DataCollectionReadPermission",
                        "DataProcessingReadPermission",
                        "RouteOptimizationFullPermission",
                        "SupportsReadPermission",
                        "NotificationsReadPermission",
                        "NotificationsReadPermission"
                    }
                }
        };
    }
}