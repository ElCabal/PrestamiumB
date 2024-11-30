using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prestamium.Entities;

namespace Prestamium.Persistence.Configurations
{
    public class InstallmentConfiguration : IEntityTypeConfiguration<Installment>
    {
        public void Configure(EntityTypeBuilder<Installment> builder)
        {
            builder.ToTable("Installment");

            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.PaidAmount).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasOne(e => e.Loan)
                .WithMany(e => e.Installments)
                .HasForeignKey(e => e.LoanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
