using MC.Models.attributes;
using MC.Models.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models
{
    [MongoTable(Collections.Likes)]
    public class Like : BaseEntity
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PostId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
