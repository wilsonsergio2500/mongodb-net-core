using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MC.AmazonStoreS3.Utils
{
    public static class ImageConverter
    {

        public static MemoryStream FromBase62ToStream(string base64Image) {
            string only64BaseCode = base64Image.Replace("data:image/png;base64,", "");
            byte[] data = Convert.FromBase64String(only64BaseCode);
            MemoryStream ms = new MemoryStream(data);
            return ms;
        }

    }
}
