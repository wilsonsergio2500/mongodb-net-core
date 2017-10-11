using MC.Interfaces.Repository;
using MC.Models;
using MC.MongoWorker.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using MC.Interfaces.client;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MC.MongoWorker.Repository
{
    public class InviteRepository : BaseRepository<Invite>, IInviteRepository
    {
        public InviteRepository(IMongoClientContext mongoClientContext) : base(mongoClientContext)
        {
        }

        public override async Task<string> Add(Invite entity) {

            entity.InviteStatus = InviteStatus.Pending;
            entity.CreatedDate = Helpers.JsDateConverter.Convert(DateTime.UtcNow);
            entity.Active = true;
            await Items.InsertOneAsync(entity);
            return entity.id;
           
        }

        public override async Task<Invite> Get(string Id) {
            Invite item = default(Invite);
            var query = Builders<Invite>.Filter.Eq(g => g.id, Id);
            item = await Items.Find(query).SingleAsync();
            return item;
        }

        public  async Task<bool> IsValid(string Id) {
            bool valid = false;
            Invite Item = await Get(Id);
            if (Item != null) {
                valid =  Item.InviteStatus == InviteStatus.Pending;
            }
            return valid;
        }

        public async Task<bool> CompleteInvite(string Id) {
            try
            {
                var query = Builders<Invite>.Filter.Eq(g => g.id, Id);
                var update = Builders<Invite>.Update.Set(g => g.InviteStatus, InviteStatus.Completed)
                                                    .Set(g => g.Active, false);

                await Items.UpdateOneAsync(query, update);
                return true;
            }
            catch {
                return false;
            }

        }

        public async Task<bool> CancelInvite(string Id) {
            try
            {
                var query = Builders<Invite>.Filter.Eq(g => g.id, Id);
                var update = Builders<Invite>.Update.Set(g => g.InviteStatus, InviteStatus.Cancelled)
                                                    .Set(g => g.Active, false);
                await Items.UpdateOneAsync(query, update);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
