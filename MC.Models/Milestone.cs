using MC.Models.attributes;
using MC.Models.Base;
using MC.Models.enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models
{
    [MongoTable(Collections.Milestones)]
    public class Milestone : BaseEntity
    {
        public string Theme { get; set; }

        public string PostContent { get; set; }

        public string Image { get; set; }

        public MilestoneType Type { get; set; }

        public List<Category> Categories { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }

    }

   
}
