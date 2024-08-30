using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Models;

public partial class SDirectContext : DbContext
{
    public SDirectContext()
    {
    }

    public SDirectContext(DbContextOptions<SDirectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PUser> PUsers { get; set; }

    public virtual DbSet<PayPalTransaction> PayPalTransactions { get; set; }

    public virtual DbSet<StripeTransaction> StripeTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__P_USER__1788CCAC109CBF41");

            entity.ToTable("P_USER");

            entity.HasIndex(e => e.Email, "UQ__P_USER__A9D1053433551026").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FacebookId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FacebookID");
            entity.Property(e => e.GoogleId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GoogleID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PayPalTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__PayPalTr__55433A6B7A8AF31D");

            entity.ToTable("PayPalTransaction");

            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PayerId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.PayPalTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PayPalTra__UserI__168F36CB");
        });

        modelBuilder.Entity<StripeTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__StripeTr__55433A6B0EC43484");

            entity.ToTable("StripeTransaction");

            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BalanceTransactionId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PaymentIntentId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethodBrand)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethodId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ReceiptUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShippingAddress).HasColumnType("text");
            entity.Property(e => e.SourceId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.StripeTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StripeTra__UserI__70698DE3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
