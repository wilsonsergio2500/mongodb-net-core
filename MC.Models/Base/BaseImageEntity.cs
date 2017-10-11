using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Models.Base
{
    public class BaseImageEntity : BaseEntity
    {

        public string ImageBlob { get; set; }

        public long CreatedOn { get; set; }
    }
}
