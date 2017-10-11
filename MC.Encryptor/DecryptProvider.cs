using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MC.Encryptor
{
    public class DecryptProvider : IDecryptProvider
    {

        public string Decrypt(string cypherText, string encryptionKey) {


            string key = encryptionKey;
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


            var fullCipher = Convert.FromBase64String(cypherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var keyBytes = Encoding.UTF8.GetBytes(encryptionKey);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(keyBytes, iv))
                {

                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }


        }
    }
}
