using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Encryptor
{
    public interface IDecryptProvider
    {
        string Decrypt(string cypherText, string encryptionKey);
    }
}
