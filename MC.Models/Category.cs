using MC.Models.attributes;
using MC.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models
{
    [MongoTable(Collections.CategoryTypes)]
    public class Category : BaseEntity
    {
        public string Name { get; set; }


    }
    
}
