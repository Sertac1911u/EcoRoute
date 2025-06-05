using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace EcoRoute.IdentityServer.Tools
{
    public static class JwtTokenGenerator
    {
        public static TokenResponseViewModel GenerateToken(GetCheckAppUserViewModel user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                new Claim("Username", user.Username ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                switch (role)
                {
                    case "SuperAdmin":
                        claims.Add(new Claim("scope", "DataCollectionFullPermission"));
                        claims.Add(new Claim("scope", "DataProcessingFullPermission"));
                        claims.Add(new Claim("scope", "DataCollectionReadPermission"));
                        claims.Add(new Claim("scope", "RouteOptimizationFullPermission"));
                        claims.Add(new Claim("scope", "SupportsFullPermission")); 
                        claims.Add(new Claim("scope", "SupportsReadPermission")); 
                        claims.Add(new Claim("scope", "SettingsFullPermission")); 
                        claims.Add(new Claim("scope", "OcelotFullPermission"));
                        claims.Add(new Claim("scope", "ReportsFullPermission"));
                        claims.Add(new Claim("scope", "NotificationsFullPermission"));
                        claims.Add(new Claim("scope", "NotificationsReadPermission"));

                        break;
                    case "Manager":
                        claims.Add(new Claim("scope", "DataCollectionFullPermission"));
                        claims.Add(new Claim("scope", "DataCollectionReadPermission"));
                        claims.Add(new Claim("scope", "RouteOptimizationFullPermission"));
                        claims.Add(new Claim("scope", "RouteOptimizationFullAccess"));
                        claims.Add(new Claim("scope", "SupportsFullPermission")); 
                        claims.Add(new Claim("scope", "SupportsReadPermission"));
                        claims.Add(new Claim("scope", "SettingsFullPermission"));
                        claims.Add(new Claim("scope", "ReportsFullPermission"));
                        claims.Add(new Claim("scope", "NotificationsFullPermission"));
                        claims.Add(new Claim("scope", "NotificationsReadPermission"));

                        break;
                    case "Driver":
                    case "Customer":
                        claims.Add(new Claim("scope", "DataCollectionFullPermission"));
                        claims.Add(new Claim("scope", "DataCollectionReadPermission"));
                        claims.Add(new Claim("scope", "DataProcessingReadPermission"));
                        claims.Add(new Claim("scope", "RouteOptimizationFullPermission"));
                        claims.Add(new Claim("scope", "SupportsReadPermission")); 
                        claims.Add(new Claim("scope", "SupportsFullPermission"));
                        claims.Add(new Claim("scope", "SettingsFullPermission"));
                        claims.Add(new Claim("scope", "ReportsFullPermission"));
                        claims.Add(new Claim("scope", "NotificationsReadPermission"));
                        claims.Add(new Claim("scope", "NotificationsFullPermission"));

                        break;
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationDate = DateTime.UtcNow.AddDays(JwtTokenDefaults.Expire);
            var token = new JwtSecurityToken(
                issuer: JwtTokenDefaults.ValidIssuer,
                audience: JwtTokenDefaults.ValidAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expirationDate,
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);
            return new TokenResponseViewModel(tokenString, expirationDate);
        }
    }
}