using MC.Interfaces.client;
using MC.Interfaces.Repository.Base;
using MC.Models.attributes;
using MC.Models.Base;
using MC.MongoWorker.Helpers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.MongoWorker.Repository.Base
{
    public class BaseImageRepository<T> : IBaseImageRepository<T> where T : BaseImageEntity, new()
    {

        protected static IMongoClient mongoClient { get; set; }
        protected static IMongoDatabase mongoDatabase { get; set; }
        protected static IMongoCollection<T> Items { get; set; }

        public BaseImageRepository(IMongoClientContext mongoClientContext)
        {
            mongoClient = mongoClientContext.getContext();
            mongoDatabase = mongoClientContext.getDatabase();
            string collection = AttributeFinder.GetAttributeValue<T, MongoTable, string>(z => z.Name);

            if (String.IsNullOrEmpty(collection))
            {
                throw new Exception(Errors.UNDEFINED_COLLECTION);
            }

            Items = mongoDatabase.GetCollection<T>(collection);

        }

        public async Task<string> Add(T entity)
        {

            entity.CreatedOn = JsDateConverter.Convert(DateTime.UtcNow);
            if (String.IsNullOrEmpty(entity.id)) {

                await Items.InsertOneAsync(entity);
                return entity.id;
                
            }

            var query = Builders<T>.Filter.Eq(g => g.id, entity.id);
            await Items.ReplaceOneAsync(query, entity);
            return entity.id;
            
        }

        public async Task<bool> Delete(string Id)
        {
            try
            {
                var query = Builders<T>.Filter.Eq(g => g.id, Id);
                await Items.FindOneAndDeleteAsync(query);
                return true;
            }
            catch {
                return false;
            }

        }

        public async Task<T> Get(string Id)
        {
            var query = Builders<T>.Filter.Eq(g => g.id, Id);
            T item = await Items.Find(query).SingleAsync();
            return item;
        }

        public async Task<string> GetImage(string Id) {
            var query = Builders<T>.Filter.Eq(g => g.id, Id);
            T item = await Items.Find(query).SingleAsync();
            return item.ImageBlob;
        }
    }
}
