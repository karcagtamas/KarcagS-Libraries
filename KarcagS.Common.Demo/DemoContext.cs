using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using KarcagS.API.Data.Entities;

namespace KarcagS.Common.Demo;

public class DemoContext(DbContextOptions<DemoContext> options) : DbContext(options)
{
    public DbSet<DemoEntry> Entries { get; set; } = default!;
    public DbSet<GenderEntry> Genders { get; set; } = default!;

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

public class DemoEntry : Entity<string>
{
    [Key]
    [Required]
    public override string Id { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

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

public class GenderEntry : Entity<int>
{
    [Key]
    [Required]
    public override int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<DemoEntry> Entries { get; set; } = default!;
    public virtual ICollection<DemoEntry> OtherEntries { get; set; } = default!;
}