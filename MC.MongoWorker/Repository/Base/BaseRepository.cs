using MC.Interfaces.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MC.MongoWorker.Helpers;
using MC.Models.attributes;
using MongoDB.Driver;
using MC.Models.Base;
using MC.Interfaces.client;
using MC.Interfaces.Repository.Base;

namespace MC.MongoWorker.Repository.Base
{

    public class BaseRepository<T> : IBaseRepository<T> where T: BaseEntity, new()
    {
        

        protected static IMongoClient mongoClient { get; set; }
        protected static IMongoDatabase mongoDatabase { get; set; }

        protected static IMongoCollection<T> Items { get; set; }

        public BaseRepository(IMongoClientContext mongoClientContext) {
            mongoClient = mongoClientContext.getContext();
            mongoDatabase = mongoClientContext.getDatabase();
            string collection = AttributeFinder.GetAttributeValue<T, MongoTable, string>(z => z.Name);

            if (String.IsNullOrEmpty(collection)) {
                throw new Exception(Errors.UNDEFINED_COLLECTION);
            }

            Items = mongoDatabase.GetCollection<T>(collection);

        }

        public virtual async Task<T> Get(string Id) {
            T item = default(T);
            var query = Builders<T>.Filter.Eq(g => g.id, Id);
            item = await Items.Find(query).SingleAsync();
            return item;

        }
        public virtual async Task<Boolean> Delete(string Id) {
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
        public virtual async Task<string> Add(T entity) {
            try
            {
                if (String.IsNullOrEmpty(entity.id))
                {
                    entity.CreatedDate = JsDateConverter.Convert(DateTime.UtcNow);
                    entity.Active = true;

                    await Items.InsertOneAsync(entity); ;
                    return entity.id.ToString();
                }
                
                var query = Builders<T>.Filter.Eq(g => g.id, entity.id);
                await Items.ReplaceOneAsync(query, entity);
                return entity.id;

            }
            catch {
                return null;
            }
        }

        public virtual async Task Add(IEnumerable<T> entity) {
            await Items.InsertManyAsync(entity);
        }

        public virtual async Task<List<T>> Get(int skip, int limit) {
          List<T> items =  await Items.Find(Builders<T>.Filter.Empty).SortByDescending(g => g.CreatedDate).Skip(skip).Limit(limit).ToListAsync<T>();
            return items;

        }

        public virtual async Task<long> GetTotal() {
            FilterDefinition<T> filter = Builders<T>.Filter.Empty;
            long count = await Items.CountAsync(filter);
            return count;

        }

        public virtual async Task<List<T>> GetAll() {
            FilterDefinition<T> filter = Builders<T>.Filter.Empty;
            return await Items.Find(filter).ToListAsync<T>();

        }

        public virtual async Task Update(T entity) {
            var query = Builders<T>.Filter.Eq(g => g.id, entity.id);
            await Items.ReplaceOneAsync(query, entity);
        }
    }
}
