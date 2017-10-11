using MC.Interfaces.Repository.Base;
using MC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Interfaces.Repository
{
    public interface IMilestoneRepository : IBaseRepository<Milestone>
    {
        Task<List<Milestone>> GetByUser(string userId, int skip, int take);

        Task<long> GetTotalByUser(string userId);

        Task<List<Milestone>> GetByCategory(string userId, string categoryId, int skip, int limit);

        Task<long> GetTotalByCategory(string userId, string categoryId);
    }
}
