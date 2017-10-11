using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mdls = MC.Models;
using MC.Interfaces.Repository.Base;
using MC.Interfaces.Repository;
using MongoCoreNet.Models;
using MongoCoreNet.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MongoCoreNet.Controllers
{
    [Produces("application/json")]
    [Route("api/Like")]
    public class LikeController : Controller
    {
        private readonly ILikeRepository likeRepository;
        private readonly IAuthtenticationCurrentContext authenticationContext;
        
        public LikeController(ILikeRepository likeRepo, IAuthtenticationCurrentContext authContext)
        {
            likeRepository = likeRepo;
            authenticationContext = authContext;
        }

        [HttpPost("like/{milestoneId}")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ActionResponse> Like(string milestoneId) {

            string userId = authenticationContext.CurrentUser;

            bool added = await likeRepository.AddLike(milestoneId, userId);

            return new ActionResponse
            {
                State = added
            };
        }

        [HttpDelete("unlike/{milestoneId}")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ActionResponse> Unlike(string milestoneId) {
            string userId = authenticationContext.CurrentUser;

            bool removed = await likeRepository.RemoveLike(milestoneId, userId);
            return new ActionResponse
            {
                State = removed
            };
        }

    }
}