using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;

namespace Prestamium.Persistence.Configurations
{
    public class BoxTransactionConfiguration : IEntityTypeConfiguration<BoxTransaction>
    {
        public void Configure(EntityTypeBuilder<BoxTransaction> builder)
        {
            builder.ToTable("BoxTransaction");
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired();
            builder.Property(e => e.Type).HasMaxLength(50).IsRequired();
            builder.Property(e => e.TransactionDate).IsRequired();
            builder.Property(e => e.PreviousBalance).HasColumnType("decimal(18,2)");
            builder.Property(e => e.NewBalance).HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Box)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.BoxId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
