using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCoreNet.Helpers
{
    public interface IUserCache
    {
        Task<DTOs.User> GetUser(string userId);
    }
}
