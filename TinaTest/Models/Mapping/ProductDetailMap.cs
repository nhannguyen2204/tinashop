using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaTest.Models.Mapping
{
    public class ProductDetailMap : EntityTypeConfiguration<ProductDetail>
    {
        public ProductDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProductCode, t.AddressId, t.ColorKey });

            // Properties
            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.AddressId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ColorKey)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("ProductDetail");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.AddressId).HasColumnName("AddressId");
            this.Property(t => t.ColorKey).HasColumnName("ColorKey");
            this.Property(t => t.Quantity).HasColumnName("Quantity");

            // Relationships
            this.HasRequired(t => t.Address)
                .WithMany(t => t.ProductDetails)
                .HasForeignKey(d => d.AddressId);
            this.HasRequired(t => t.Color)
                .WithMany(t => t.ProductDetails)
                .HasForeignKey(d => d.ColorKey);
            this.HasRequired(t => t.Product)
                .WithMany(t => t.ProductDetails)
                .HasForeignKey(d => d.ProductCode);

        }
    }
}
