using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Interfaces.client
{
    public interface IMongoClientContext
    {
        IMongoClient getContext();

        IMongoDatabase getDatabase();
    }
}
