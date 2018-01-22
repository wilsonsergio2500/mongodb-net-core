using System;
using System.Collections.Generic;
using System.Text;
using MC.AmazonStoreS3.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.IO;
using MC.AmazonStoreS3.Utils;
using Amazon.S3.Model;

namespace MC.AmazonStoreS3.Providers
{
    public class AmazonS3ImageProvider : AmazonStoreS3Provider, IAmazonS3ImageProvider
    {
        public AmazonS3ImageProvider(IOptions<AmazonStoreS3Config> config) : base(config)
        {
        }

        public async Task<string> Add(string key, string base64Image){
            
                MemoryStream stm = ImageConverter.FromBase62ToStream(base64Image);
                string filePath = await this.GetSave($"{key}", stm);
                return filePath;
           
        }

        public override async Task<bool> Delete(string key) {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest
                {
                    BucketName = this.Config.Bucket,
                    Key = $"{key}"
                };

                await this.s3Client.DeleteObjectAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual string GetContentType()
        {
            return "image/png";
        }

    }
}
