using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaTest.Models.Mapping
{
    public class ColorMap : EntityTypeConfiguration<Color>
    {
        public ColorMap()
        {
            // Primary Key
            this.HasKey(t => t.ColorKey);

            // Properties
            this.Property(t => t.ColorKey)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ColorCode)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.CreatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UpdatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Color");
            this.Property(t => t.ColorKey).HasColumnName("ColorKey");
            this.Property(t => t.ColorCode).HasColumnName("ColorCode");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.UpdatedUserId).HasColumnName("UpdatedUserId");
            this.Property(t => t.CreatedDatetime).HasColumnName("CreatedDatetime");
            this.Property(t => t.UpdatedDatetime).HasColumnName("UpdatedDatetime");
        }
    }
}
