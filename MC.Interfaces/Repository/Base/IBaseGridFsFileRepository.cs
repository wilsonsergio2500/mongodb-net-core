using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MC.Interfaces.Repository.Base
{
    public interface IBaseGridFsFileRepository
    {
        Task<string> Add(string filename, byte[] source);

        Task<string> Add(string filename, FileStream fileStream);

        Task<bool> Delete(string Id);

        Task<byte[]> Get(string Id);

    }
}
