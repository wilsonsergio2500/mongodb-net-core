using System;
using System.Collections.Generic;
using System.Text;

namespace MC.AmazonStoreS3.Models
{
    public class AmazonStoreS3Config
    {
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Bucket { get; set; }
        public string Path { get; set; }
    }
}
