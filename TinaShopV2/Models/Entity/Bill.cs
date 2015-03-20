using System;
using System.Collections.Generic;

namespace TinaShopV2.Models.Entity
{
    public partial class Bill
    {
        public Bill()
        {
            this.BillDetails = new List<BillDetail>();
        }

        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public int ItemTotal { get; set; }
        public decimal PriceTotal { get; set; }
        public int BillTypeId { get; set; }
        public int StoreAddressId { get; set; }
        public string ClientId { get; set; }
        public string StaffId { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public bool IsDeleted { get; set; }
        public virtual BillType BillType { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
