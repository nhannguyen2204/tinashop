using System;

namespace TinaShopV2.Models.Entity
{
    public partial class TinaMenu
    {
        public int Id { get; set; }
        public int MenuTypeId { get; set; }
        public string Name { get; set; }
        public string CssClass { get; set; }
        public Nullable<int> ActionId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool IsHidden { get; set; }
        public int OrderNumber { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual MenuType MenuType { get; set; }
        public virtual TinaAction TinaAction { get; set; }
    }
}