using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Cache
{
    public interface ICacheProvider
    {
        Task<T> AddAsync<T>(string key, T item);
        bool DoesKeyExist<T>(string key);
        T Get<T>(string Key);
        void Set<T>(string key, T item);

    }
}
