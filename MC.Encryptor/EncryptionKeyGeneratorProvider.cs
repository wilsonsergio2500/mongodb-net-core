using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Encryptor
{
    public class EncryptionKeyGeneratorProvider : EncryptionProvider , IEncryptionKeyGeneratorProvider
    {
        public EncryptionKeyGeneratorProvider(IDecryptProvider decryptionService) : base(Helpers.KeyGenerator.NewKey, decryptionService)
        {
        }

        public string EncryiptionKey => GetCurrentKey();
    }
}
