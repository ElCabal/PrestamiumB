using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prestamium.Entities;

namespace Prestamium.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");
            builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.DocumentNumber).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Phone).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.Email).HasMaxLength(100);

            builder.HasIndex(e => e.DocumentNumber).IsUnique();
        }
    }
}
