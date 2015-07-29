using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaShopV2.Models.Entity.Mapping
{
    public class CatalogMap : EntityTypeConfiguration<Catalog>
    {
        public CatalogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            //// Properties
            //this.Property(t => t.Id)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.CreatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UpdatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Catalog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MediaId).HasColumnName("MediaId");
            this.Property(t => t.IsPublished).HasColumnName("IsPublished");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.UpdatedUserId).HasColumnName("UpdatedUserId");
            this.Property(t => t.CreatedDatetime).HasColumnName("CreatedDatetime");
            this.Property(t => t.UpdatedDatetime).HasColumnName("UpdatedDatetime");

            // Relationships
            this.HasMany(t => t.Products)
                .WithMany(t => t.Catalogs)
                .Map(m =>
                {
                    m.ToTable("CatalogDetail");
                    m.MapLeftKey("CatalogId");
                    m.MapRightKey("ProductCode");
                });


        }
    }
}