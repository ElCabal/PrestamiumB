using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prestamium.Entities;

namespace Prestamium.Persistence.Configurations
{
    public class InstallmentConfiguration : IEntityTypeConfiguration<Installment>
    {
        public void Configure(EntityTypeBuilder<Installment> builder)
        {
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.DueDate).HasColumnType("datetime");   
        }
    }
}
