using Microsoft.EntityFrameworkCore;
using W1.Server.Models;

namespace W1.Server;

public class W1DbContext : DbContext 
{
    public DbSet<W1Name> Names { get; set; }

    public W1DbContext(DbContextOptions<W1DbContext> contextOptions) : base(contextOptions) { }
}
