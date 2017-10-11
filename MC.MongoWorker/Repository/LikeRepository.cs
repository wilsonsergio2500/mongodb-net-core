using MC.MongoWorker.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using MC.Interfaces.client;
using MC.Models;
using MC.Interfaces.Repository;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MC.MongoWorker.Repository
{
    public class LikeRepository : BaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(IMongoClientContext mongoClientContext) : base(mongoClientContext)
        {
        }

        public async Task<bool> RemoveLike(string PostId, string UserId) {

            FilterDefinition<Like> byPost = Builders<Like>.Filter.Eq(x => x.PostId, PostId);
            FilterDefinition<Like> byUser = Builders<Like>.Filter.Eq(x => x.UserId, UserId);

            FilterDefinition<Like> and = Builders<Like>.Filter.And(byPost, byUser);

            Like likeItem = await Items.Find(and).SingleAsync();
            bool deleted = await this.Delete(likeItem.id);

            return deleted;

        }

        public async Task<bool> HasLike(string PostId, string UserId) {
            try
            {
                FilterDefinition<Like> byPost = Builders<Like>.Filter.Eq(x => x.PostId, PostId);
                FilterDefinition<Like> byUser = Builders<Like>.Filter.Eq(x => x.UserId, UserId);

                FilterDefinition<Like> and = Builders<Like>.Filter.And(byPost, byUser);

                Like likeItem = await Items.Find(and).SingleAsync();
                return likeItem != null;
            }catch
            {
                return false;
            }
        }

        public async Task<bool> AddLike(string PostId, string UserId) {

            Like like = new Like
            {
                PostId = PostId,
                UserId = UserId

            };

            string Id = await this.Add(like);
            return Id != null;
        }
    }
}
