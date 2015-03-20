using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaShopV2.Models.Entity.Mapping
{
    public class BillDetailMap : EntityTypeConfiguration<BillDetail>
    {
        public BillDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BillId, t.ProductCode, t.StoreAddressId, t.ColorId });

            // Properties
            this.Property(t => t.BillId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.StoreAddressId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ColorId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("BillDetail");
            this.Property(t => t.BillId).HasColumnName("BillId");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.StoreAddressId).HasColumnName("StoreAddressId");
            this.Property(t => t.ColorId).HasColumnName("ColorId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.CashDiscount).HasColumnName("CashDiscount");

            // Relationships
            this.HasRequired(t => t.Bill)
                .WithMany(t => t.BillDetails)
                .HasForeignKey(d => d.BillId);

        }
    }
}
