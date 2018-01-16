using MC.Models;
using MC.MongoWorker.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using MC.Interfaces.client;
using System.Threading.Tasks;
using MC.MongoWorker.Helpers;
using MongoDB.Driver;
using MC.Interfaces.Repository;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace MC.MongoWorker.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IMongoClientContext mongoClientContext) : base(mongoClientContext)
        {
        }

        //public override async Task<string> Add(Category entity) {
        //    entity.Active = true;
        //    if (String.IsNullOrEmpty(entity.id)) {
        //        await Items.InsertOneAsync(entity);
        //        return entity.id;
        //    }

        //    FilterDefinition<Category> query = Builders<Category>.Filter.Eq(g => g.id, entity.id);
        //    await Items.ReplaceOneAsync(query, entity);
        //    return entity.id;
        //}

        public async Task<bool> DoesNameExist(string Name)
        {

            FilterDefinition<Category> queryActiveOnly = Builders<Category>.Filter.Eq(g => g.Active, true);
            FilterDefinition<Category> queryhasName = Builders<Category>.Filter.Eq(g => g.Name, Name);

            FilterDefinition<Category> queryall = Builders<Category>.Filter.And(queryActiveOnly, queryhasName);

            long count = await Items.CountAsync(queryall);
            return count != 0;
            

        }

        public async Task<List<Category>> MatchRecordsByName(string Name)
        {
            FilterDefinition<Category> queryActiveOnly = Builders<Category>.Filter.Eq(g => g.Active, true);
            
            FilterDefinition<Category> queryContainName = Builders<Category>.Filter.Regex(g => g.Name,
                BsonRegularExpression.Create(new Regex(Name, RegexOptions.IgnoreCase)) 
                );
                
            FilterDefinition<Category> queryall = Builders<Category>.Filter.And(queryActiveOnly, queryContainName);

            List<Category> categories = await Items.Find(queryall).ToListAsync<Category>();
            return categories;
        }
    }
}
