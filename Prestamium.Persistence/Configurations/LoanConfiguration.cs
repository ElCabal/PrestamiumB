using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prestamium.Entities;

namespace Prestamium.Persistence.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalAmountDue).HasColumnType("decimal(18,2)");
            builder.Property(x => x.StartDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.EndDate).HasColumnType("datetime");
        }
    }
}
    