using Amazon.S3;
using Amazon.S3.Model;
using MC.AmazonStoreS3.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MC.AmazonStoreS3.Providers
{
    public abstract class AmazonStoreS3Provider : IAmazonStoreS3Provider
    {
        protected readonly AmazonStoreS3Config Config;
        protected readonly AmazonS3Client s3Client;
            
        public AmazonStoreS3Provider(IOptions<AmazonStoreS3Config> config)
        {
            this.Config = config.Value;
            this.s3Client = new AmazonS3Client(this.Config.Key, this.Config.Secret, Amazon.RegionEndpoint.USWest2);
        }

        public virtual async Task<MemoryStream> Get(string key) {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = this.Config.Bucket,
                    Key = key
                };
                using (GetObjectResponse response = await this.s3Client.GetObjectAsync(request))
                {
                    MemoryStream stream = new MemoryStream();
                    await response.ResponseStream.CopyToAsync(stream);
                    return stream;
                }
            }
            catch {
                return null;
            }
        }

        public virtual async Task<string> GetSave(string key, MemoryStream stm)
        {
            try
            {
                bool saved = await this.Save(key, stm);
                if (saved) {
                    return $"{this.Config.Path}/{this.Config.Bucket}/{key}";
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public virtual async Task<bool> Save(string key, MemoryStream stm)
        {
            try
            {
                PutObjectRequest objectRequest = new PutObjectRequest
                {
                    BucketName = this.Config.Bucket,
                    Key = key,
                    InputStream = stm,
                    ContentType = this.GetContentType()
                };
                await this.s3Client.PutObjectAsync(objectRequest);
                return true;

            }
            catch {
                return false;
            }
        }

        public virtual async Task<bool> Delete(string key) {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest
                {
                    BucketName = this.Config.Bucket,
                    Key = key
                };

                await this.s3Client.DeleteObjectAsync(request);
                return true;
            }
            catch {
                return false;
            }
        }

        

     

        public void Dispose()
        {
            this.s3Client.Dispose();
        }

        public virtual string GetContentType()
        {
            return "text/plain";
        }

        
    }
}
