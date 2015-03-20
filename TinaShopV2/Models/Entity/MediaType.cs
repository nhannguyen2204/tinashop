using System;
using System.Collections.Generic;

namespace TinaShopV2.Models.Entity
{
    public partial class MediaType
    {
        public MediaType()
        {
            this.Media = new List<Media>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual ICollection<Media> Media { get; set; }
    }
}