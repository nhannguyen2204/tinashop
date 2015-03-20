using System;
using System.Collections.Generic;

namespace TinaShopV2.Models.Entity
{
    public partial class TinaAuthorize
    {
        public string RoleId { get; set; }
        public int ActionId { get; set; }
        public virtual TinaAction TinaAction { get; set; }
    }
}
