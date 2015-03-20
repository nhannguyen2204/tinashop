using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaShopV2.Models.Entity.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductCode);

            // Properties
            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.BrandCode)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.CreatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UpdatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Product");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.BrandCode).HasColumnName("BrandCode");
            this.Property(t => t.CanSale).HasColumnName("CanSale");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.IsPublish).HasColumnName("IsPublish");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.UpdatedUserId).HasColumnName("UpdatedUserId");
            this.Property(t => t.CreatedDatetime).HasColumnName("CreatedDatetime");
            this.Property(t => t.UpdatedDatetime).HasColumnName("UpdatedDatetime");

            // Relationships
            this.HasRequired(t => t.Brand)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.BrandCode);

        }
    }
}
