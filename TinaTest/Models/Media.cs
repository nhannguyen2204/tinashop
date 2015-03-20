using System;
using System.Collections.Generic;

namespace TinaTest.Models
{
    public partial class Media
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string ThumbPath { get; set; }
        public int TypeId { get; set; }
        public string ProductCode { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual MediaType MediaType { get; set; }
        public virtual Product Product { get; set; }
    }
}
