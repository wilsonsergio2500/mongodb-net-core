using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MC.Interfaces.Repository;
using AutoMapper;
using Mdls = MC.Models;
using MongoCoreNet.Models;
using MongoCoreNet.Authorization;
using Microsoft.AspNetCore.Authorization;
using MC.Encryptor;
using MongoCoreNet.Models.Profile;
using MC.AmazonStoreS3.Providers;

namespace MongoCoreNet.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthtenticationCurrentContext authenticationCurrentContext;
        private readonly IEncryptionKeyGeneratorProvider encryptionProvider;
        private readonly IDecryptProvider decryptionProvider;
        private readonly IMapper mapper;
        private readonly IAmazonS3ImageProvider amazonS3ImageProvider;
        public UserController(IUserRepository userRepo, IAuthtenticationCurrentContext currentAuthContext, IMapper autoMapper, 
            IEncryptionKeyGeneratorProvider encryptionP, IDecryptProvider decryptProvider, IAmazonS3ImageProvider amazons3imageprovider)
        {
            userRepository = userRepo;
            authenticationCurrentContext = currentAuthContext;
            mapper = autoMapper;
            encryptionProvider = encryptionP;
            decryptionProvider = decryptProvider;
            amazonS3ImageProvider = amazons3imageprovider;

        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId) {

            Mdls.User user = await userRepository.Get(userId);
            DTOs.User userClient = mapper.Map<Mdls.User, DTOs.User>(user);

            return Ok(userClient);

        }

        [HttpGet("me")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<DTOs.User> GetMe() {

            string userId = authenticationCurrentContext.CurrentUser;

            Mdls.User user = await userRepository.Get(userId);
            DTOs.User userClient = mapper.Map<Mdls.User, DTOs.User>(user);

            return userClient;

        }

        [HttpPut("update/me/image")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ActionResponse> UpdateImage([FromBody]ImageEdit imageRequest) {

            string userId = authenticationCurrentContext.CurrentUser;

            if (imageRequest.Image != null && imageRequest.Image.StartsWith("data:image/png;base64,"))
            {
                string path = await amazonS3ImageProvider.Add($"users/{userId}/profile.png", imageRequest.Image);
                imageRequest.Image = path;
            }

            bool updated = await userRepository.UpdateImage(userId, imageRequest.Image);
            return new ActionResponse
            {
                State = updated
            };
        }

        [HttpPut("update/me/password")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ActionResponse> UpdatePassword([FromBody]PasswordEdit passwordRequest) {
            string userId = authenticationCurrentContext.CurrentUser;

            Mdls.User user = await userRepository.Get(userId);

            if (user != null)
            {
                #region Verify Password Match
                string originalPassword = decryptionProvider.Decrypt(user.Password, user.EncryptionKey);
                bool verified = passwordRequest.CurrentPassword == originalPassword;
                #endregion

                if (verified)
                {
                    string encryptedPassword = encryptionProvider.Encrypt(passwordRequest.Password);
                    string encryptionKey = encryptionProvider.EncryiptionKey;

                    bool updated = await userRepository.UpdatePassword(userId, encryptedPassword, encryptionKey);

                    return new ActionResponse
                    {
                        State = updated
                    };
                }
            }

            return new ActionResponse
            {
                State = false
            };

            
        }

        [HttpPut("update/me/bio")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ActionResponse> UpdateBio([FromBody]BioEdit bioRequest) {

            string userId = authenticationCurrentContext.CurrentUser;
            bool updated = await userRepository.UpdateBio(userId, bioRequest.Bio, bioRequest.JobTitle, bioRequest.Strengths);

            return new ActionResponse {
                State = updated
            }; 
        }

        [HttpGet("role/me")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public UserRoleResponse GetUserRole() {
            return new UserRoleResponse
            {
                RoleId = authenticationCurrentContext.CurrentRoleId
            };
        }

        [HttpPost("check/username/used")]
        public async Task<ActionResponse> CheckUserNameUsed([FromBody]UserNameUsed payload) {

            bool IsUsed = await userRepository.DoesUserNameExist(payload.UserName);
            return new ActionResponse
            {
                State = IsUsed
            };
        }

        #region Admin Based Task

        [HttpGet("list/{skip}/{take}")]
        [Authorize(Policy = Policies.AUTHORIZATION_ADMIN_ONLY)]
        public async Task<ListResponse<DTOs.User>> GetUserList(int skip, int take) {

            long count = await userRepository.GetTotal();

            List<Mdls.User> usersm = await userRepository.Get(skip, take);

            List<DTOs.User> usersd = mapper.Map<List<Mdls.User>, List<DTOs.User>>(usersm);

            return new ListResponse<DTOs.User>
            {
                Count = count,
                Result = usersd
            };

        }

        [HttpPost("deactivate")]
        [Authorize(Policy = Policies.AUTHORIZATION_ADMIN_ONLY)]
        public async Task<ActionResponse> Deactivate([FromBody]ActivationBasedRequest request) {

            bool completed = await userRepository.DeactivateUserByEmail(request.Email);
            return new ActionResponse
            {
                State = completed
            };
        }

        [HttpPost("activate")]
        [Authorize(Policy = Policies.AUTHORIZATION_ADMIN_ONLY)]
        public async Task<ActionResponse> Activate([FromBody]ActivationBasedRequest request) {

            bool completed = await userRepository.ActivateUserByEmail(request.Email);
            return new ActionResponse
            {
                State = completed
            };
        }

        [HttpPost("update/user/role")]
        [Authorize(Policy = Policies.AUTHORIZATION_ADMIN_ONLY)]
        public async Task<ActionResponse> UpdateUserRole([FromBody]RoleChangeBasedRequest request) {
            bool completed = await userRepository.SetRoleByEmail(request.email, request.role);

            return new ActionResponse
            {
                State = completed
            };
        }

        #endregion

    }
}