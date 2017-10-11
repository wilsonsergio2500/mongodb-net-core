using MC.Interfaces.Context;
using MC.Models;
using MC.MongoWorker.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MC.Interfaces.client;

namespace MC.MongoWorker.Repository
{
   

    public class PostTypeRepository : BaseRepository<PostType>
    {
        public PostTypeRepository(IMongoClientContext mongoClientContext) : base(mongoClientContext)
        {
        }
    }
}
