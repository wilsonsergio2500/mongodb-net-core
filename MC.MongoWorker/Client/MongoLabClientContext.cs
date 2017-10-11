using MC.Interfaces.client;

using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MC.MongoWorker.Models;

namespace MC.MongoWorker.client
{
    public class MongoLabClientContext : IMongoClientContext
    {
        private IMongoClient mongoClient { get; set; }
        private IMongoDatabase mongoDatabase { get; set; }
        public MongoLabClientContext(IOptions<MongoLabConfig> config)
        {
            MongoLabConfig configuration = config.Value;
            string connection = $"mongodb://{configuration.User}:{configuration.Password}@{configuration.EndPoint}";
            MongoUrl mongoUrl = new MongoUrl(connection);
            mongoClient = new MongoClient(mongoUrl);
            mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            
        }

        public IMongoClient getContext()
        {
            return mongoClient;
        }


        public IMongoDatabase getDatabase()
        {
            return mongoDatabase;
        }
    }
}
