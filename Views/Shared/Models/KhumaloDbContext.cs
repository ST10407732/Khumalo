using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KhumaloCraftKC.Models
{
    public partial class KhumaloDbContext : IdentityDbContext<IdentityUser>
    {
        public KhumaloDbContext(DbContextOptions<KhumaloDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().ToTable("Order"); // Specify table name for Order entity

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFA0834EDA");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Orders__ProductI__4E88ABD4");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__PRODUCT__B40CC6EDD6C8C00B");

                entity.ToTable("PRODUCT");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.Category)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ImageURL");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PK__TRANSACT__55433A4BE1DFCAF3");

                entity.ToTable("TRANSACTIONS");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.TransactionDate).HasColumnType("datetime");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Product).WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__TRANSACTI__Produ__52593CB8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-KhumaloCraftKC-07aea343-3481-45c3-a21a-2a9127d73ae9;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
