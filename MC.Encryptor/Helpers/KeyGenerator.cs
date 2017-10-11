using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MC.Encryptor.Helpers
{
    internal static class KeyGenerator
    {
         public static string NewKey {
            get
            {

                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, 32)
                  .Select(s => s[random.Next(s.Length)]).ToArray());


            }
        }
    }
}
