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
using MongoCoreNet.Helpers;

namespace MongoCoreNet.Controllers
{
    [Produces("application/json")]
    [Route("api/Like")]
    public class LikeController : Controller
    {
        private readonly ILikeRepository likeRepository;
        private readonly IAuthtenticationCurrentContext authenticationContext;
        private readonly IUserCache userCache;
        
        public LikeController(ILikeRepository likeRepo, IAuthtenticationCurrentContext authContext, IUserCache ucache)
        {
            likeRepository = likeRepo;
            authenticationContext = authContext;
            userCache = ucache;
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

        [HttpGet("post/count/{postId}")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<TotalResponse> GetPostCount(string postId) {

            long count = await likeRepository.GetPostCount(postId);

            return new TotalResponse {
                Count = count
            };
        }

        [HttpGet("post/likes/most5/{postId}")]
        public async Task<ListResponse<DTOs.User>> GetRecent(string postId) {

            Task<List<Mdls.Like>> LikeListTask = Task.Run<List<Mdls.Like>>(() => likeRepository.GetRecent5(postId));
            Task<long> LikeCountTask = Task.Run<long>(() => likeRepository.GetPostCount(postId));
            await Task.WhenAll(LikeListTask, LikeCountTask);

            List<Task<DTOs.User>> userTask = new List<Task<DTOs.User>>();

            foreach (Mdls.Like like in LikeListTask.Result) {
                userTask.Add(userCache.GetUser(like.UserId));
            }

            List<DTOs.User> users = new List<DTOs.User>();
            while (userTask.Count > 0) {
                Task<DTOs.User> user = await Task.WhenAny(userTask.ToArray());
                users.Add(user.Result);
                userTask.Remove(user);
            }

            return new ListResponse<DTOs.User>
            {
                Count = LikeCountTask.Result,
                Result = users
            };



        }
    }
}