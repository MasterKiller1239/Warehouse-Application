using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Models;

namespace WarehouseApplication.Data
{
    public class WarehouseContext : DbContext, IWarehouseContext
    {
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contractor>(entity =>
            {
                entity.ToTable("contractors");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Symbol).HasColumnName("symbol");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("documents");
                entity.HasKey(d => d.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.Symbol).HasColumnName("symbol");
                entity.Property(e => e.ContractorId).HasColumnName("contractorid");
                entity.HasOne(d => d.Contractor)
                        .WithMany()
                        .HasForeignKey(d => d.ContractorId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DocumentItem>(entity =>
            {
                entity.ToTable("document_items");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ProductName).HasColumnName("productname");
                entity.Property(e => e.Unit).HasColumnName("unit");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.DocumentId).HasColumnName("documentid");
            });
        }
        public virtual DbSet<Contractor> Contractors { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentItem> DocumentItems { get; set; }
    }
}
