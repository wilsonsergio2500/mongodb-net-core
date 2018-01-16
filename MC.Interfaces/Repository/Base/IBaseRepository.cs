using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Interfaces.Repository.Base
{
    public interface IBaseRepository<T>
    {
        Task<T> Get(string Id);
        Task<Boolean> Delete(string Id);
        Task<string> Add(T entity);
        Task Add(IEnumerable<T> entity);
        Task<List<T>> Get(int skip, int limit);

        Task<List<T>> GetAll();

        Task<long> GetTotal();
        Task Update(T entity);

        Task<Boolean> Activate(string Id);

        Task<Boolean> Deactivate(string Id);
        

        Task CreateIndex();
    }
}
