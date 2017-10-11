using MC.Models.attributes;
using MC.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models
{
    [MongoTable(Collections.PostTypes)]
    public class PostType : BaseEntity
    {
        public CategoryType CategoryTypeId { get; set; }

        public string Name { get; set; }
    }
}
