using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models.attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoTable : Attribute
    {
        public bool IsReadOnly { get; set; }
        public string Name { get; set; }
        public MongoTable(string name) {
            Name = name;
        }
    }
}
