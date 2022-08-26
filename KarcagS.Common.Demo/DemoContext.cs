using KarcagS.Common.Tools.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KarcagS.Common.Demo;

public class DemoContext : DbContext
{
    public DbSet<DemoEntry> Entries { get; set; } = default!;
    public DbSet<GenderEntry> Genders { get; set; } = default!;

    public DemoContext(DbContextOptions<DemoContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DemoEntry>()
            .HasOne(x => x.Gender)
            .WithMany(x => x.Entries)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}

public class DemoEntry : IEntity<string>
{
    [Key]
    [Required]
    public string Id { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public int Age { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int GenderId { get; set; }

    public virtual GenderEntry Gender { get; set; } = default!;
}

public class GenderEntry : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

    public virtual ICollection<DemoEntry> Entries { get; set; } = default!;
}
