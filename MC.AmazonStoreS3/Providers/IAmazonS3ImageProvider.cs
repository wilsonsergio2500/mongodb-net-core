using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.AmazonStoreS3.Providers
{
    public interface IAmazonS3ImageProvider : IAmazonStoreS3Provider
    {
        Task<string> Add(string key, string base64Image);
    }
}
