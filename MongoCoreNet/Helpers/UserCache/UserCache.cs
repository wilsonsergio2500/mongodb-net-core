using AutoMapper;
using MC.Cache;
using MC.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mdls = MC.Models;

namespace MongoCoreNet.Helpers
{
    public class UserCache : IUserCache
    {
        private readonly ICacheProvider cacheProvider;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UserCache(ICacheProvider cache, IUserRepository userRepo, IMapper automapper) {
            cacheProvider = cache;
            userRepository = userRepo;
            mapper = automapper;
        }

        public async Task<DTOs.User> GetUser(string userId)
        {

            if (cacheProvider.DoesKeyExist<DTOs.User>(userId))
            {
                DTOs.User user = cacheProvider.Get<DTOs.User>(userId);
                return user;
            }
            else
            {
                Mdls.User userdisplay = await userRepository.Get(userId);
                DTOs.User user = mapper.Map<Mdls.User, DTOs.User>(userdisplay);
                cacheProvider.Set<DTOs.User>(userId, user);
                return user;
            }
        }
    }
}
