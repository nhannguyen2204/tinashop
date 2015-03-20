using System;
using System.Collections.Generic;

namespace TinaTest.Models
{
    public partial class TinaAuthorize
    {
        public string RoleId { get; set; }
        public int ActionId { get; set; }
        public virtual TinaAction TinaAction { get; set; }
    }
}
