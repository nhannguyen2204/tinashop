using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaShopV2.Models.Entity.Mapping
{
    public class BillMap : EntityTypeConfiguration<Bill>
    {
        public BillMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ClientName)
                .HasMaxLength(150);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(50);

            this.Property(t => t.ClientId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.StaffId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.CreatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UpdatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Bill");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ClientName).HasColumnName("ClientName");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ItemTotal).HasColumnName("ItemTotal");
            this.Property(t => t.PriceTotal).HasColumnName("PriceTotal");
            this.Property(t => t.BillTypeId).HasColumnName("BillTypeId");
            this.Property(t => t.StoreAddressId).HasColumnName("StoreAddressId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.StaffId).HasColumnName("StaffId");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.UpdatedUserId).HasColumnName("UpdatedUserId");
            this.Property(t => t.CreatedDatetime).HasColumnName("CreatedDatetime");
            this.Property(t => t.UpdatedDatetime).HasColumnName("UpdatedDatetime");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");

            // Relationships
            this.HasRequired(t => t.BillType)
                .WithMany(t => t.Bills)
                .HasForeignKey(d => d.BillTypeId);

        }
    }
}
