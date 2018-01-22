using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MC.AmazonStoreS3.Providers
{
    public interface IAmazonStoreS3Provider : IDisposable
    {
        Task<MemoryStream> Get(string key);
        Task<bool> Save(string key, MemoryStream stm);
        Task<string> GetSave(string key, MemoryStream stm);
        Task<bool> Delete(string key);
        string GetContentType();
    }
}
