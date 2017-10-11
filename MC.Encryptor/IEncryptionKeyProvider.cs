using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Encryptor
{
    public interface IEncryptionKeyGeneratorProvider : IEncryptionProvider
    {

         string EncryiptionKey { get; }

    }
}
