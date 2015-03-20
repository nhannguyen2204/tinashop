using System;
using System.Collections.Generic;

namespace TinaTest.Models
{
    public partial class TinaAction
    {
        public TinaAction()
        {
            this.TinaAuthorizes = new List<TinaAuthorize>();
            this.TinaMenus = new List<TinaMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsAnonymous { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual ICollection<TinaAuthorize> TinaAuthorizes { get; set; }
        public virtual ICollection<TinaMenu> TinaMenus { get; set; }
    }
}
