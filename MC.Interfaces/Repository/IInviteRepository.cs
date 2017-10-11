using MC.Interfaces.Repository.Base;
using MC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Interfaces.Repository
{
    public interface IInviteRepository : IBaseRepository<Invite>
    {

        Task<bool> IsValid(string Id);
        Task<bool> CompleteInvite(string Id);

        Task<bool> CancelInvite(string Id);
    }
}
