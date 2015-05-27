using System;
using System.Collections.Generic;

namespace TinaShopV2.Models.Entity
{
    public partial class Slider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Nullable<int> MediaId { get; set; }
        public int OrderNumber { get; set; }
        public bool IsPublished { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
    }
}