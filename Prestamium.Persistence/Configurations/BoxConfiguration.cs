using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Prestamium.Persistence.Configurations
{
    public class BoxConfiguration : IEntityTypeConfiguration<Box>
    {
        public void Configure(EntityTypeBuilder<Box> builder)
        {
            builder.ToTable("Box");
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.InitialBalance).HasColumnType("decimal(18,2)");
            builder.Property(e => e.CurrentBalance).HasColumnType("decimal(18,2)");
        }
    }
}
