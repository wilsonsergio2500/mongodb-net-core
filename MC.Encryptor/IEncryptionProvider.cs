using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Encryptor
{
    public interface IEncryptionProvider
    {
        string Encrypt(string text);
        string Decrypt(string cypherText);

        string GetCurrentKey();

    }
}
