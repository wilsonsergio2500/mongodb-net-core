using MC.JwToken.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using MC.JwToken.ModelToken;

namespace MC.JwToken
{
    public class JwtSecurityProvider : IJwtSecurityProvider
    {
        private readonly string Secret;
        private readonly string Issuer;
        private readonly string Audience;
        private readonly TimeSpan Expiration;
        private readonly SigningCredentials Credentials;
        private readonly TokenValidationParameters TokenValidationParams;

        private DateTimeOffset TokenExpiration
        {
            get
            {
                return DateTimeOffset.UtcNow.Add(Expiration);
            }
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity Identity)
        {

            SecurityTokenDescriptor TokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = Audience,
                Issuer = Issuer,
                NotBefore = DateTime.UtcNow,
                Expires = TokenExpiration.UtcDateTime,
                Subject = Identity,
                SigningCredentials = Credentials,
            };

            return TokenDescriptor;

        }

        public JwtSecurityProvider(IOptions<AuthConfig> configuration)
        {
            AuthConfig config = configuration.Value;
            Secret = config.secret;
            Issuer = config.Issuer;
            Audience = config.Audience;
            Expiration = TimeSpan.FromHours(config.hours);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            TokenValidationParams = new TokenValidationParameters()
            {
                ValidAudiences = new string[] { Audience, Issuer },
                ValidIssuer = Issuer,
                IssuerSigningKey = securityKey

            };

        }

        public string WriteToken(ClaimsIdentity Identity)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = GetTokenDescriptor(Identity);
            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

        public string WriteToken<T>(IModelTokenGenerator<T> tokenGenerator)
        {

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity Identity = tokenGenerator.getClaims;
            SecurityTokenDescriptor descriptor = GetTokenDescriptor(Identity);
            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);

        }

        public ReadOnlyCollection<Claim> GetClaimsCollection(string encodedToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token;
          
            ClaimsPrincipal claimPrincipal = tokenHandler.ValidateToken(encodedToken, TokenValidationParams, out token);
            return new ReadOnlyCollection<Claim>(claimPrincipal.Claims.ToList());
        }

        public bool IsTokenValid(string encodedToken) {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token;
                ClaimsPrincipal claims = tokenHandler.ValidateToken(encodedToken, TokenValidationParams, out token);
                return claims.Claims.Count() > 0;
            }
            catch {
                return false;
            }
        }

        
    }
}
