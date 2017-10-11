using MC.Interfaces.Repository.Base;
using MC.Models;
using MC.MongoWorker.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using MC.Interfaces.client;
using MC.Interfaces.Repository;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MC.MongoWorker.Repository
{
    public class MilestoneRepository : BaseRepository<Milestone>, IMilestoneRepository
    {
        public MilestoneRepository(IMongoClientContext mongoClientContext) : base(mongoClientContext)
        {
        }

        public async Task<List<Milestone>> GetByUser(string userId, int skip, int limit)
        {
            FilterDefinition<Milestone> query = Builders<Milestone>.Filter.Eq(x => x.UserId, userId);
            List<Milestone> elements = await Items.Find(query).SortByDescending(g => g.CreatedDate).Skip(skip).Limit(limit).ToListAsync<Milestone>();
            return elements;
        }

        public async Task<List<Milestone>> GetByCategory(string userId, string categoryId, int skip, int limit) {
            FilterDefinition<Milestone> queryId = Builders<Milestone>.Filter.Eq(x => x.UserId, userId);
            FilterDefinition<Milestone> queryType = Builders<Milestone>.Filter.Eq(x => x.Type, MC.Models.enums.MilestoneType.LandMark);
            FilterDefinition<Milestone> queryMatch = Builders<Milestone>.Filter.ElemMatch(x => x.Categories, x => x.id == categoryId);

            FilterDefinition<Milestone> and = Builders<Milestone>.Filter.And(queryId, queryType, queryMatch);
            List<Milestone> elements = await Items.Find(and).SortByDescending(g => g.CreatedDate).Skip(skip).Limit(limit).ToListAsync<Milestone>();

            return elements;
            
        }

        public async Task<long> GetTotalByUser(string userId)
        {
            FilterDefinition<Milestone> query = Builders<Milestone>.Filter.Eq(x => x.UserId, userId);
            long count = await Items.CountAsync(query);
            return count;
        }

        public async Task<long> GetTotalByCategory(string userId, string categoryId) {

            FilterDefinition<Milestone> queryId = Builders<Milestone>.Filter.Eq(x => x.UserId, userId);
            FilterDefinition<Milestone> queryType = Builders<Milestone>.Filter.Eq(x => x.Type, MC.Models.enums.MilestoneType.LandMark);
            FilterDefinition<Milestone> queryMatch = Builders<Milestone>.Filter.ElemMatch(x => x.Categories, x => x.id == categoryId);

            FilterDefinition<Milestone> and = Builders<Milestone>.Filter.And(queryId, queryType, queryMatch);
            long count = await Items.CountAsync(and);
            return count;
        }
        

    }
}
