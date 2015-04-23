using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaTest.Models.Mapping
{
    public class MediaMap : EntityTypeConfiguration<Media>
    {
        public MediaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(150);

            this.Property(t => t.FilePath)
                .HasMaxLength(250);

            this.Property(t => t.ThumbPath)
                .HasMaxLength(250);

            this.Property(t => t.ProductCode)
                .HasMaxLength(10);

            this.Property(t => t.CreatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UpdatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Media");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.FilePath).HasColumnName("FilePath");
            this.Property(t => t.ThumbPath).HasColumnName("ThumbPath");
            this.Property(t => t.TypeId).HasColumnName("TypeId");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.UpdatedUserId).HasColumnName("UpdatedUserId");
            this.Property(t => t.CreatedDatetime).HasColumnName("CreatedDatetime");
            this.Property(t => t.UpdatedDatetime).HasColumnName("UpdatedDatetime");

            // Relationships
            this.HasRequired(t => t.MediaType)
                .WithMany(t => t.Medias)
                .HasForeignKey(d => d.TypeId);
            this.HasOptional(t => t.Product)
                .WithMany(t => t.Medias)
                .HasForeignKey(d => d.ProductCode);

        }
    }
}
