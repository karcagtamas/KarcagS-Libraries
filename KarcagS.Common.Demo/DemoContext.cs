using KarcagS.Common.Tools.Entities;
using KarcagS.Shared.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KarcagS.Common.Demo;

public class DemoContext : DbContext
{
    public DbSet<DemoEntry> Entries { get; set; } = default!;

    public DemoContext(DbContextOptions<DemoContext> options) : base(options)
    {

    }
}

public class DemoEntry : IEntity<string>, IIdentified<string>
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
    public bool Male { get; set; }
}
