using Microsoft.EntityFrameworkCore;
using Mise.Entities;

namespace Mise.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
        .HasMany(e => e.Tags)
        .WithMany(e => e.Products);

        modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();


    }

}