using Microsoft.EntityFrameworkCore;

namespace SistemaDeEventos.Models;

public class EventosContext : DbContext
{
    public EventosContext(DbContextOptions<EventosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tabela User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("tb_user");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.CreatedAt)
                  .HasColumnName("created_at")
                  .HasDefaultValueSql("now()");
        });

        // Tabela Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("tb_order");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt)
                  .HasColumnName("created_at")
                  .HasDefaultValueSql("now()");
            entity.Property(e => e.PaymentType).HasMaxLength(100);
            entity.Property(e => e.Total)
                  .HasColumnType("numeric(10,2)");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Orders)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Tabela Rating
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("tb_rating");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt)
                  .HasColumnName("created_at")
                  .HasDefaultValueSql("now()");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.Note);

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Ratings)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Order)
                  .WithMany()
                  .HasForeignKey(d => d.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
