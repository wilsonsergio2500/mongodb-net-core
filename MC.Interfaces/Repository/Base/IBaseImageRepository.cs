using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Interfaces.Repository.Base
{
    public interface IBaseImageRepository<T>
    {
        Task<string> Add(T entity);
        Task<bool> Delete(string Id);

        Task<T> Get(string Id);

        Task<string> GetImage(string Id);
    }
}
