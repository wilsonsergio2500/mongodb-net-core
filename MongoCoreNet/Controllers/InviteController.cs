using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mdls = MC.Models;
using MC.Interfaces.Repository;
using AutoMapper;
using MongoCoreNet.Models;
using MC.Encryptor;
using MC.Email;
using MC.Email.Utils;
using MC.AmazonStoreS3.Providers;

namespace MongoCoreNet.Controllers
{

    [Route("api/Invite")]
    public class InviteController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IInviteRepository inviteRepository;
        private readonly IEncryptionKeyGeneratorProvider encryptionProvider;
        private readonly IEmailProvider emailProvider;
        private readonly IAmazonS3ImageProvider amazonS3ImageProvider;
        public InviteController(IUserRepository userRepo, IInviteRepository inviteRepo, IEncryptionKeyGeneratorProvider encryptionGP, IEmailProvider emailService,
            IAmazonS3ImageProvider amazons3imageprovider
            ) {
            userRepository = userRepo;
            inviteRepository = inviteRepo;
            encryptionProvider = encryptionGP;
            emailProvider = emailService;
            amazonS3ImageProvider = amazons3imageprovider;
        }

        [HttpGet("{id}")]

        public async Task<Mdls.Invite> Get(string id) {

            Mdls.Invite invite = await inviteRepository.Get(id);
            return invite;
        }


        [HttpPost("new/{email}/{roleType}")]
        public async Task<IActionResult> Post(string email, int roleType)
        {
            

            

            bool emailExist = await userRepository.DoesEmailExist(email);
            if (!emailExist)
            {
                
                Mdls.Invite invite = new Mdls.Invite
                {
                    Email = email,
                    ParticipationRoleType = (Mdls.RoleType)roleType
                };

                string inviteId = await inviteRepository.Add(invite);
                await emailProvider.SendEmailAsHtml(email, EmailTemplate.getInviteTemplate(inviteId));


            }
            else {
                return BadRequest(new DTOs.Error(ErrorResponses.NVITE_USER_EXIST));
            }

            return Ok(new ActionResponse { State = true });

        }

        [HttpGet("records/{skip}/{take}")]
        public async Task<ListResponse<Mdls.Invite>> GetInvites(int skip, int take) {

            long count = await inviteRepository.GetTotal();
            List<Mdls.Invite> invites = await inviteRepository.Get(skip, take);

            return new ListResponse<Mdls.Invite>
            {
                Count = count,
                Result = invites
            };

        }
        [HttpPost("cancel/{id}")]
        public async Task<ActionResponse> Cancel(string id) {

            var cancelled = await inviteRepository.CancelInvite(id);

            return new ActionResponse { State = cancelled };

        }

        [HttpPost("complete")]
        public async Task<IActionResult> Complete([FromBody]DTOs.InviteCompletion completionRequest)
        {

            ActionResponse actionResponse = new ActionResponse() {
                State = false
            };
            //check if inviation is active
            Mdls.Invite invite = await inviteRepository.Get(completionRequest.InvitationId);
            if (invite.InviteStatus == Mdls.InviteStatus.Pending) {

                string encryptedPassword = encryptionProvider.Encrypt(completionRequest.User.Password);
                string encryptionKey = encryptionProvider.EncryiptionKey;

                completionRequest.User.Password = encryptedPassword;
                completionRequest.User.EncryptionKey = encryptionKey;

                

                string userId = await userRepository.Add(completionRequest.User);
                #region Image
                if (completionRequest.User.Image != null && completionRequest.User.Image.StartsWith("data:image/png;base64,"))
                {
                    string path = await amazonS3ImageProvider.Add($"users/{userId}/profile.png", completionRequest.User.Image);
                    bool saved = await userRepository.UpdateImage(userId, path);
                }
                #endregion
                if (!String.IsNullOrEmpty(userId)) {

                    bool updated = await inviteRepository.CompleteInvite(completionRequest.InvitationId);
                    actionResponse.State = updated;
                }
            }
            else if (invite.InviteStatus == Mdls.InviteStatus.Completed) {

                return BadRequest(new DTOs.Error(ErrorResponses.INVITE_COMPLETED_EXIST));
            }

            return Ok(actionResponse);
        }
    }
}