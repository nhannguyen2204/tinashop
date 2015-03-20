using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaShopV2.Models.Entity.Mapping
{
    public class TinaActionMap : EntityTypeConfiguration<TinaAction>
    {
        public TinaActionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Area)
                .HasMaxLength(150);

            this.Property(t => t.Controller)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.Action)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.CreatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UpdatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("TinaAction");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Area).HasColumnName("Area");
            this.Property(t => t.Controller).HasColumnName("Controller");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.IsAnonymous).HasColumnName("IsAnonymous");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.UpdatedUserId).HasColumnName("UpdatedUserId");
            this.Property(t => t.CreatedDatetime).HasColumnName("CreatedDatetime");
            this.Property(t => t.UpdatedDatetime).HasColumnName("UpdatedDatetime");
        }
    }
}
