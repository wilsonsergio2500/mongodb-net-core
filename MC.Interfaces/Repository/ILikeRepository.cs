using MC.Interfaces.Repository.Base;
using MC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Interfaces.Repository
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        Task<bool> RemoveLike(string PostId, string UserId);

        Task<bool> HasLike(string PostId, string UserId);
        Task<bool> AddLike(string PostId, string UserId);
    }
}
