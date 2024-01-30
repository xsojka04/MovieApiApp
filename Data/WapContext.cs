using Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Data;
public class WapContext : DbContext
{
  public DbSet<User> Users { get; set; }
  public DbSet<LikedPerson> LikedPeople { get; set; }
  public DbSet<LikedMovie> LikedMovies { get; set; }
  public DbSet<LikedTv> LikedTvs { get; set; }

    public WapContext(DbContextOptions<WapContext> options) : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
  }
}

