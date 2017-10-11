using MC.Encryptor;
using MC.JwToken.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using MC.JwToken.ModelToken;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace MC.JwToken.Providers
{
    public class TokenEncryptionProvider : EncryptionProvider, ITokenEncryptionProvider
    {
        private readonly IJwtSecurityProvider jwtSecurityProvider;
        public TokenEncryptionProvider(
            IOptions<AuthConfig> config, 
            IDecryptProvider decryptionService,
            IJwtSecurityProvider jwtProvider
            )  : base(config.Value.tokenKey, decryptionService)
        {
            jwtSecurityProvider = jwtProvider;
            
        }

        public ReadOnlyCollection<Claim> GetClaimsCollection(string encryptedToken)
        {
            string decryptedToken = Decrypt(encryptedToken);
            return jwtSecurityProvider.GetClaimsCollection(decryptedToken);
        }

        public bool IsTokenValid(string encryptedToken)
        {
            string decryptedToken = Decrypt(encryptedToken);
            return jwtSecurityProvider.IsTokenValid(decryptedToken);
        }

        public string WriteToken(ClaimsIdentity Identity)
        {
            string token = jwtSecurityProvider.WriteToken(Identity);
            string encrypted = Encrypt(token);
            return encrypted;
        }

        public string WriteToken<T>(IModelTokenGenerator<T> tokenGenerator)
        {
            string token = jwtSecurityProvider.WriteToken<T>(tokenGenerator);
            string encrypted = Encrypt(token);
            return encrypted;

        }
    }
}
