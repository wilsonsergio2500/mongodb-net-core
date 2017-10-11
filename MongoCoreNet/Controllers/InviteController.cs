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

namespace MongoCoreNet.Controllers
{

    [Route("api/Invite")]
    public class InviteController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IInviteRepository inviteRepository;
        private readonly IEncryptionKeyGeneratorProvider encryptionProvider;
        public InviteController(IUserRepository userRepo, IInviteRepository inviteRepo, IEncryptionKeyGeneratorProvider encryptionGP) {
            userRepository = userRepo;
            inviteRepository = inviteRepo;
            encryptionProvider = encryptionGP;
        }

        [HttpGet("{id}")]

        public async Task<Mdls.Invite> Get(string id) {

            Mdls.Invite invite = await inviteRepository.Get(id);
            return invite;
        }


        [HttpPost("new/{email}/{roleType}")]
        public async Task<ActionResponse> Post(string email, int roleType)
        {

            bool emailExist = await userRepository.DoesEmailExist(email);
            if (!emailExist) {
                Mdls.Invite invite = new Mdls.Invite {
                    Email = email,
                    ParticipationRoleType =(Mdls.RoleType)roleType
                };

                //MC.Models.Invite inviteModel = mapper.Map<DTOs.Invite, MC.Models.Invite>(invite);
                string inviteId = await inviteRepository.Add(invite);


            }

            return new ActionResponse { State = true };

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
                if (!String.IsNullOrEmpty(userId)) {

                    bool updated = await inviteRepository.CompleteInvite(completionRequest.InvitationId);
                    actionResponse.State = updated;
                }
            }
            else if (invite.InviteStatus == Mdls.InviteStatus.Completed) {

                return BadRequest(new DTOs.Error("Invite has already been Completed"));
            }

            return Ok(actionResponse);
        }
    }
}