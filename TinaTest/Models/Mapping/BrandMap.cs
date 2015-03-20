using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaTest.Models.Mapping
{
    public class BrandMap : EntityTypeConfiguration<Brand>
    {
        public BrandMap()
        {
            // Primary Key
            this.HasKey(t => t.BrandCode);

            // Properties
            this.Property(t => t.BrandCode)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.CreatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UpdatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.SortName)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Brand");
            this.Property(t => t.BrandCode).HasColumnName("BrandCode");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.UpdatedUserId).HasColumnName("UpdatedUserId");
            this.Property(t => t.CreatedDatetime).HasColumnName("CreatedDatetime");
            this.Property(t => t.UpdatedDatetime).HasColumnName("UpdatedDatetime");
            this.Property(t => t.SortName).HasColumnName("SortName");
        }
    }
}
