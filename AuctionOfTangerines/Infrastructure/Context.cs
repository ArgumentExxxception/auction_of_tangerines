using Core.Entities;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class Context: DbContext
{
    public DbSet<Client?> Clients { get; set; }
    public DbSet<Bet?> Bets { get; set; }
    
    public DbSet<Tangerine> Tangerines { get; set; }
    
    public Context(DbContextOptions<Context> options): base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}