using MC.Models;
using MC.MongoWorker.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using MC.Interfaces.client;

namespace MC.MongoWorker.Repository
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(IMongoClientContext mongoClientContext) : base(mongoClientContext)
        {
        }
    }
}
