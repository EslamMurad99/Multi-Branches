using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MultiBranches.Models;

namespace MultiBranches.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BranchModel> TbBranches { get; set; }
        public DbSet<ProductModel> TbProducts { get; set; }
        public DbSet<BranchProductModel> TbBranchProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BranchProductModel>()
                .HasKey(bp => new { bp.BranchId, bp.ProductId });

            modelBuilder.Entity<BranchProductModel>()
                .HasOne(bp => bp.Branch)
                .WithMany(b => b.BranchProducts)
                .HasForeignKey(bp => bp.BranchId);

            modelBuilder.Entity<BranchProductModel>()
                .HasOne(bp => bp.Product)
                .WithMany(p => p.BranchProducts)
                .HasForeignKey(bp => bp.ProductId);
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}
