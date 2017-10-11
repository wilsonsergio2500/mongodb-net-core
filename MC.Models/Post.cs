using MC.Models.attributes;
using MC.Models.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models
{
    [MongoTable(Collections.Posts)]
    public class Post : BaseEntity
    {
        public string PostContent { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string OwnerId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ImageId { get; set; }

    }
}
