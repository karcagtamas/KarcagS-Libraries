using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using KarcagS.API.Data;

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

        modelBuilder.Entity<DemoEntry>()
            .HasOne(x => x.OtherGender)
            .WithMany(x => x.OtherEntries)
            .OnDelete(DeleteBehavior.SetNull);

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

    public int? OtherGenderId { get; set; }

    public virtual GenderEntry Gender { get; set; } = default!;
    public virtual GenderEntry? OtherGender { get; set; }
}

public class GenderEntry : IEntity<int>
{
    [Key]
    [Required]
    public int Id { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

    public virtual ICollection<DemoEntry> Entries { get; set; } = default!;
    public virtual ICollection<DemoEntry> OtherEntries { get; set; } = default!;
}