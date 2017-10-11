using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCoreNet.Models
{
    public class ListResponse<T>
    {
        public ListResponse()
        {
            Result = new List<T>();
        }

        public long Count { get; set; }

        public IList<T> Result { get; set; }
    }
}
