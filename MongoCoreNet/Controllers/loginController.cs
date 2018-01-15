using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MC.Interfaces.Repository;
using Mdls = MC.Models;
using MC.JwToken.ModelToken;
using MC.Encryptor;
using MC.JwToken;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using MongoCoreNet.Authorization;

namespace MongoCoreNet.Controllers
{
    [Produces("application/json")]
    [Route("api/login")]
    public class loginController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IModelTokenGenerator<Mdls.User> tokenGenerator;
        private readonly IDecryptProvider decryptionProvider;
        private readonly IJwtSecurityProvider tokenProvider;
        public loginController(IUserRepository userRepo, IModelTokenGenerator<Mdls.User> tokenG, IDecryptProvider decryptProvider, IJwtSecurityProvider tokenp)
        {
            userRepository = userRepo;
            tokenGenerator = tokenG;
            decryptionProvider = decryptProvider;
            tokenProvider = tokenp;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DTOs.UserCredentials credentials)
        {

            Mdls.User user = await userRepository.GetUserByNameOrEmail(credentials.Name);

            if (user != null && user.Active)
            {
                string Password = decryptionProvider.Decrypt(user.Password, user.EncryptionKey);

                if (credentials.Password == Password)
                {

                    Dictionary<string, Func<Mdls.User, object>> contract = new Dictionary<string, Func<Mdls.User, object>>() {
                    { ClaimKeys.USER_ID, (Mdls.User u) => u.id },
                    { ClaimKeys.ROLE, (Mdls.User u) => (int)u.Role}
                    };

                    tokenGenerator.Create(user, contract);
                    string token = tokenProvider.WriteToken<Mdls.User>(tokenGenerator);
                    

                    return Ok(new { token = token });

                }

            }



            return BadRequest(new DTOs.Error("Error On User Credentials"));


        }

        [HttpGet("IsAuthenticated")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public IActionResult IsAuthenticated()
        {
            return Ok();

        }



    }
}