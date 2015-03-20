﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TinaShopV2.Models.Entity
{
    public partial class MenuType
    {
        public MenuType()
        {
            this.TinaMenus = new List<TinaMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual ICollection<TinaMenu> TinaMenus { get; set; }
    }
}