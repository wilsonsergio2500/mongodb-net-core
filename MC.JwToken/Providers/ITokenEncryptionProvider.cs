using MC.Encryptor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.JwToken.Providers
{
    public interface ITokenEncryptionProvider : IJwtSecurityProvider, IEncryptionProvider
    {
    }
}
