using System;
using System.Collections.Generic;
using System.Text;

namespace MC.MongoWorker.Models
{
    public class MongoAzureConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string DbName { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public string MongoCredentialMechanism { get; set; }

    }
}
