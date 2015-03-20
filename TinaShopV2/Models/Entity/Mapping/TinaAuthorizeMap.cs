using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaShopV2.Models.Entity.Mapping
{
    public class TinaAuthorizeMap : EntityTypeConfiguration<TinaAuthorize>
    {
        public TinaAuthorizeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RoleId, t.ActionId });

            // Properties
            this.Property(t => t.RoleId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.ActionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TinaAuthorize");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.ActionId).HasColumnName("ActionId");

            // Relationships
            this.HasRequired(t => t.TinaAction)
                .WithMany(t => t.TinaAuthorizes)
                .HasForeignKey(d => d.ActionId);

        }
    }
}
