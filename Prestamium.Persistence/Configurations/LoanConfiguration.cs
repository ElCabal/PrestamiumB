using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prestamium.Entities;

namespace Prestamium.Persistence.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loan");

            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.InterestRate).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.TotalAmountDue).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.TotalInterestReceivable).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.PaymentAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.RemainingBalance).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.Frequency).HasMaxLength(20).IsRequired();

            builder.HasOne(e => e.Client)
                .WithMany(e => e.Loans)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Box)
                .WithMany(e => e.Loans)
                .HasForeignKey(e => e.BoxId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
    