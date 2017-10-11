using MC.Interfaces.client;
using MC.Interfaces.Repository.Base;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MC.MongoWorker.Repository.Base
{
    
    public class BaseGridFsFileRepository : IBaseGridFsFileRepository
    {
        protected static IMongoClient mongoClient { get; set; }
        protected static IMongoDatabase mongoDatabase { get; set; }

        protected GridFSBucket gridFsBucket { get; set; }

        public BaseGridFsFileRepository(IMongoClientContext mongoClientcontext)
        {
            mongoClient = mongoClientcontext.getContext();
            mongoDatabase = mongoClientcontext.getDatabase();

            gridFsBucket = new GridFSBucket(mongoDatabase);
        }

        public virtual async Task<string> Add(string filename, byte[] source ) {

            
           MongoDB.Bson.ObjectId Id =  await gridFsBucket.UploadFromBytesAsync(filename, source);

            return Id.ToString();
        }

        public virtual async Task<string> Add(string filename, FileStream fileStream) {

           MongoDB.Bson.ObjectId Id =  await gridFsBucket.UploadFromStreamAsync(filename, fileStream);
            return Id.ToString();
        }

        public virtual async Task<bool> Delete(string Id) {
            try
            {
                await gridFsBucket.DeleteAsync(new MongoDB.Bson.ObjectId(Id));
                return true;
            }
            catch  {
                return false;
            }
        }

        public virtual async Task<byte[]> Get(string Id) {

            byte[] file =  await gridFsBucket.DownloadAsBytesAsync(new MongoDB.Bson.ObjectId(Id));
            return file;
        }

        

    }
}
