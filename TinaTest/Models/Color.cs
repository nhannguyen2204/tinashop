using System;
using System.Collections.Generic;

namespace TinaTest.Models
{
    public partial class Color
    {
        public Color()
        {
            this.ProductDetails = new List<ProductDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
