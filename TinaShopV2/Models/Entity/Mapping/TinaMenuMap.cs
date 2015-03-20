using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TinaShopV2.Models.Entity.Mapping
{
    public class TinaMenuMap : EntityTypeConfiguration<TinaMenu>
    {
        public TinaMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.CssClass)
                .HasMaxLength(150);

            this.Property(t => t.CreatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UpdatedUserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("TinaMenu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuTypeId).HasColumnName("MenuTypeId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CssClass).HasColumnName("CssClass");
            this.Property(t => t.ActionId).HasColumnName("ActionId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.IsHidden).HasColumnName("IsHidden");
            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.UpdatedUserId).HasColumnName("UpdatedUserId");
            this.Property(t => t.CreatedDatetime).HasColumnName("CreatedDatetime");
            this.Property(t => t.UpdatedDatetime).HasColumnName("UpdatedDatetime");

            // Relationships
            this.HasRequired(t => t.MenuType)
                .WithMany(t => t.TinaMenus)
                .HasForeignKey(d => d.MenuTypeId);
            this.HasOptional(t => t.TinaAction)
                .WithMany(t => t.TinaMenus)
                .HasForeignKey(d => d.ActionId);

        }
    }
}