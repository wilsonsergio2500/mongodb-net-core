using MC.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Interfaces.Context
{
    public interface IDataContext
    {
        IMongoCollection<PostType> PostTypes { get;  }
    }
}
