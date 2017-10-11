using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MC.Encryptor
{
    
    public abstract class EncryptionProvider : IEncryptionProvider
    {
        private readonly string encryptionKey;
        private readonly IDecryptProvider decryptionService;

        public EncryptionProvider(string key, IDecryptProvider decryptProvider) {
            encryptionKey = key;
            decryptionService = decryptProvider;
        }

        public string Encrypt(string text) {


            var key = encryptionKey;
            if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            else
            {
                while (key.Length < 32)
                {
                    key += "0";
                }
            }

            var keyEncoded = Encoding.UTF8.GetBytes(encryptionKey);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(keyEncoded, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }


        }

        public string Decrypt(string cypherText) {

            return decryptionService.Decrypt(cypherText, encryptionKey);

        }


        public string GetCurrentKey() {
            return encryptionKey;
        }
    }
}
