using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MC.JwToken.Models;
using MC.JwToken.Helpers;
using MC.JwToken.ModelToken;
using MC.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Collections.ObjectModel;
using MC.Encryptor;
using MC.JwToken.Providers;

namespace MC.JwToken.Test
{
    [TestClass]
    public class UnitTest1
    {
        

        [TestMethod]
        public void ConfigurationValid() {
            IServiceProvider sp = StartUp.ServiceProvider;
            IOptions<AuthConfig> config = sp.GetService<IOptions<AuthConfig>>();
            Assert.IsNotNull(config.Value);

        }

        const string UserKey = "userId";
        
        [TestMethod]
        public IModelTokenGenerator<User> GetTokenGenerator() {
            IServiceProvider sp = StartUp.ServiceProvider;
            IModelTokenGenerator<User> tokenGenerator = sp.GetService<IModelTokenGenerator<User>>();

            User user = new User
            {
                id = "597b468a7ab542adcc6d247f",
                Role = RoleType.Participant
            };
            Dictionary<string, Func<User, object>> contract = new Dictionary<string, Func<User, object>>() {
                { UserKey, (User u) => u.id },
                { ClaimTypes.Role, (User u) => (int)u.Role}
            };
           

            tokenGenerator.Create(user, contract);
            ClaimsIdentity identity = tokenGenerator.getClaims;

            Assert.IsNotNull(identity);

            return tokenGenerator;
        }

        [TestMethod]
        public void JwtSecurityProviderCreated() {
            IServiceProvider sp = StartUp.ServiceProvider;

            IJwtSecurityProvider jwtSecurityProvider = sp.GetService<IJwtSecurityProvider>();
            IModelTokenGenerator<User> TokenGenerator = GetTokenGenerator();
            string token = jwtSecurityProvider.WriteToken<User>(TokenGenerator);
            ReadOnlyCollection<Claim> claims = jwtSecurityProvider.GetClaimsCollection(token);

            Assert.IsNotNull(token);
            Assert.IsNotNull(claims.GetKey(UserKey));
            Assert.IsNotNull(claims.GetKey(ClaimTypes.Role));

            //string role = claims.GetKey(ClaimTypes.Role);

            //IEncryptionProvider encryptionProvider = sp.GetService<IEncryptionProvider>();
            //string text = "I am sergio";
            //string encrypted = encryptionProvider.Encrypt(text);
            //string decrypted = encryptionProvider.Decrypt(encrypted);

            //DecryptProvider decryptService = new DecryptProvider();
            //EncryptionKeyProvider encryptProvider = new EncryptionKeyProvider(decryptService);
            //string enc = encryptProvider.Encrypt(text);
            //string key = encryptProvider.EncryiptionKey;
            //var value = decryptService.Decrypt(enc, key);

            //Assert.IsTrue(text == decrypted);



            //Assert.IsNotNull(token);

        }

        [TestMethod]
        public void EncryptionProviderEncrypts() {

            IServiceProvider sp = StartUp.ServiceProvider;

            IEncryptionProvider encryptionProvider = sp.GetService<ITokenEncryptionProvider>();
            string text = "I am sergio";
            string encrypted = encryptionProvider.Encrypt(text);
            string decrypted = encryptionProvider.Decrypt(encrypted);

            Assert.IsTrue(text == decrypted);

        }


        [TestMethod]
        public void TokenEncryptionProviderEncrypts() {

            IServiceProvider sp = StartUp.ServiceProvider;

            ITokenEncryptionProvider encryptionProvider = sp.GetService<ITokenEncryptionProvider>();
            IModelTokenGenerator<User> tokenGenerator = GetTokenGenerator();

            string encryptedToken = encryptionProvider.WriteToken<User>(tokenGenerator);
            ReadOnlyCollection<Claim> claims = encryptionProvider.GetClaimsCollection(encryptedToken);

            Assert.IsNotNull(encryptedToken);
            Assert.IsNotNull(claims.GetKey(UserKey));
            Assert.IsNotNull(claims.GetKey(ClaimTypes.Role));

            //string text = "I am sergio";
            //string encrypted = encryptionProvider.Encrypt(text);
            //string decrypted = encryptionProvider.Decrypt(encrypted);

            //Assert.IsTrue(text == decrypted);
        }

    }
}
