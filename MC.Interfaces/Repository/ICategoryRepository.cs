using MC.Interfaces.Repository.Base;
using MC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Interfaces.Repository
{
    public interface ICategoryRepository :IBaseRepository<Category>
    {
        Task<bool> DoesNameExist(string Name);

        Task<List<Category>> MatchRecordsByName(string Name);
    }
}
