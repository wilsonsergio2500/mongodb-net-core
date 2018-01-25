using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mdls = MC.Models;
using Microsoft.AspNetCore.Mvc;
using MC.Interfaces.Repository.Base;
using MongoCoreNet.Models;
using MC.Interfaces.Repository;
using MongoCoreNet.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using MC.Cache;
using AutoMapper;
using MongoCoreNet.Helpers;
using MC.AmazonStoreS3.Providers;

namespace MongoCoreNet.Controllers
{
    [Produces("application/json")]
    [Route("api/Milestone")]
    public class MilestoneController : Controller
    {
        private readonly IMilestoneRepository milestoneRepository;
        private readonly IAuthtenticationCurrentContext currentAuthenticationContext;
        private readonly ICacheProvider cacheProvider;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly ILikeRepository likeRepository;
        private readonly IUserCache userCache;
        private readonly IAmazonS3ImageProvider amazonS3ImageProvider;

        public MilestoneController(IMilestoneRepository milestoneRepo, IAuthtenticationCurrentContext currentAuthContext, 
            ICacheProvider cacheP, IMapper automapper, IUserRepository userrepo, ILikeRepository likeRepo, IUserCache usrCache,
            IAmazonS3ImageProvider amazons3imageprovider
            )
        {
            milestoneRepository = milestoneRepo;
            currentAuthenticationContext = currentAuthContext;
            cacheProvider = cacheP;
            mapper = automapper;
            userRepository = userrepo;
            likeRepository = likeRepo;
            userCache = usrCache;
            amazonS3ImageProvider = amazons3imageprovider;


        }

        [HttpGet("{id}")]
        public async Task<Mdls.Milestone> Get(string id) {

            Mdls.Milestone milestone = await milestoneRepository.Get(id);

            return milestone;

        }

        [HttpGet("user/records/{skip}/{take}")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ListResponse<Mdls.Milestone>> GetRecordsByUser(int skip, int take) {
            string userId = currentAuthenticationContext.CurrentUser;

            long count = await milestoneRepository.GetTotalByUser(userId);
            List<Mdls.Milestone> records = await milestoneRepository.GetByUser(userId, skip, take);

            return new ListResponse<Mdls.Milestone>() {
                Count = count,
                Result = records
            };
        }

        [HttpGet("records/{skip}/{take}")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ListResponse<DTOs.GridElment>> GetRecords(int skip, int take)
        {
            string ownerId = currentAuthenticationContext.CurrentUser;

            ListResponse<DTOs.GridElment> gridElements = new ListResponse<DTOs.GridElment>();
            List<DTOs.GridElment> items = new List<DTOs.GridElment>();
            List<Task<DTOs.GridElment>> gdelments = new List<Task<DTOs.GridElment>>();

            gridElements.Count = await milestoneRepository.GetTotal();

            

            List<Mdls.Milestone> records = await milestoneRepository.Get(skip, take);
            foreach (Mdls.Milestone milestone in records)
            {

                gdelments.Add(ResolveGridElement(milestone, ownerId));
            }

            while (gdelments.Count > 0)
            {

                Task<DTOs.GridElment> finishTask = await Task.WhenAny(gdelments.ToArray());
                gdelments.Remove(finishTask);

                items.Add(finishTask.Result);
            }


            gridElements.Result = items.OrderByDescending(k => k.Milestone.CreatedDate).ToList<DTOs.GridElment>();

         

            return gridElements;
        }

     

        [HttpGet("record/item/{milestoneId}")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<DTOs.GridElment> GetRecord(string milestoneId) {

            Mdls.Milestone milestone = await milestoneRepository.Get(milestoneId);
            DTOs.User user = await userCache.GetUser(milestone.UserId);
            bool IsLiked = await likeRepository.HasLike(milestone.id, milestone.UserId);
            Models.enums.LikeType Like = IsLiked ? Models.enums.LikeType.ON : Models.enums.LikeType.OFF;

            return new DTOs.GridElment
            {
                Milestone = milestone,
                User = user,
                Like = Like,
                Self = currentAuthenticationContext.CurrentUser == milestone.UserId
            };


        }


        [HttpPost("new")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ActionResponse> Add([FromBody]Mdls.Milestone milestone) {

            milestone.UserId = currentAuthenticationContext.CurrentUser;

            string milestoneId = await milestoneRepository.Add(milestone);
            if (milestone.Image != null && milestone.Image.StartsWith("data:image/png;base64,"))
            {
                string path = await amazonS3ImageProvider.Add($"milestones/{milestone.UserId}/{milestoneId}/img.png", milestone.Image);
                bool saved = await milestoneRepository.SetImage(milestoneId, path);
            }

            return new ActionResponse()
            {
                State = !String.IsNullOrEmpty(milestoneId)
            };

        }

        [HttpGet("record/category/{userId}/{categoryId}/{skip}/{take}")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ListResponse<DTOs.GridElment>> GetRecordsByCategory(string userId, string categoryId, int skip, int take) {
            string ownerId = currentAuthenticationContext.CurrentUser;

            List<DTOs.GridElment> gridElements = new List<DTOs.GridElment>();

            List<Mdls.Milestone> milestones = await milestoneRepository.GetByCategory(userId, categoryId, skip, take);
            long count = await milestoneRepository.GetTotalByCategory(userId, categoryId);

            List<Task<DTOs.GridElment>> gdelments = new List<Task<DTOs.GridElment>>();

            foreach (Mdls.Milestone milestone in milestones) {

                gdelments.Add(ResolveGridElement(milestone, ownerId));
            }

            while (gdelments.Count > 0) {

                Task<DTOs.GridElment> finishTask = await Task.WhenAny(gdelments.ToArray());
                gdelments.Remove(finishTask);

                gridElements.Add(finishTask.Result);
            }
           

            return new ListResponse<DTOs.GridElment>
            {
                Count = count,
                Result = gridElements.OrderByDescending(k => k.Milestone.CreatedDate).ToList<DTOs.GridElment>()

            };
        }


        private async Task<DTOs.GridElment> ResolveGridElement(Mdls.Milestone milestone, string ownerId) {


            bool IsLiked = await likeRepository.HasLike(milestone.id, ownerId);
            DTOs.User user = await userCache.GetUser(milestone.UserId);

            Models.enums.LikeType Like = IsLiked ? Models.enums.LikeType.ON : Models.enums.LikeType.OFF;
            
            DTOs.GridElment ge = new DTOs.GridElment
            {
                Like = Like,
                Milestone = milestone,
                User = user,
                Self = ownerId == milestone.UserId
            };
            return ge;
        }

        [HttpDelete("remove/item/{milestoneId}")]
        [Authorize(Policy = Policies.AUTHORIZATION_TOKEN)]
        public async Task<ActionResponse> RemoveItem(string milestoneId) {

            Mdls.Milestone milestone = await milestoneRepository.Get(milestoneId);

            if (milestone != null) {
                bool isOwner = milestone.UserId == currentAuthenticationContext.CurrentUser;
                if (isOwner) {
                    bool deleted = await milestoneRepository.Delete(milestoneId);
                    bool awsdeleted = await amazonS3ImageProvider.Delete($"milestones/{milestone.UserId}/{milestone.id}/img.png");
                    return new ActionResponse
                    {
                        State = deleted
                    };
                }
            }

            return new ActionResponse {
                State = false
            };

        }
    }
}