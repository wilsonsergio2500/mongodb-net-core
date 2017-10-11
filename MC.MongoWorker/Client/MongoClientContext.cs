using MC.Interfaces.client;
using MC.MongoWorker.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.MongoWorker.client
{
    public class MongoClientContext : IMongoClientContext
    {
        private MongoClientSettings settings { get; set; }
        private MongoClient client { get; set; }
        private IMongoDatabase mongoDatabase { get; set; }

        public MongoClientContext(IOptions<MongoAzureConfig> config)
        {
            MongoAzureConfig configuration = config.Value;

            settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(configuration.Host, configuration.Port);
            settings.UseSsl = true; 
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(configuration.DbName, configuration.Username);
            MongoIdentityEvidence evidence = new PasswordEvidence(configuration.Password);

            settings.Credentials = new List<MongoCredential>() {
                new MongoCredential(configuration.MongoCredentialMechanism, identity, evidence)
            };

            client = new MongoClient(settings);
            mongoDatabase = client.GetDatabase(configuration.DbName);
        }

        public IMongoClient getContext()
        {
            return client;
        }

        public IMongoDatabase getDatabase()
        {
            return mongoDatabase;
        }
    }
}
