﻿using System;
using System.Collections.Generic;
using TinaShopV2.Models.Entity;

namespace TinaShopV2.Models.Entity
{
    public partial class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public string CatCode { get; set; }
        public string CatParentCode { get; set; }
        public int OrderNumber { get; set; }
        public Nullable<int> MediaId { get; set; }
        public string Name { get; set; }
        public bool IsPublish { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
